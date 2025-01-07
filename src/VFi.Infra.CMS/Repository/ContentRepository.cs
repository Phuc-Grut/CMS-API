using MassTransit.Initializers;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using VFi.Domain.CMS.Interfaces;
using VFi.Domain.CMS.Models;
using VFi.Infra.CMS.Context;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Exceptions;
using VFi.NetDevPack.Queries;
using VFi.NetDevPack.Utilities;

namespace VFi.Infra.CMS.Repository;

public class ContentRepository : IContentRepository
{
    protected readonly SqlCoreContext Db;
    protected readonly DbSet<Content> DbSet;
    public IUnitOfWork UnitOfWork => Db;
    public ContentRepository(SqlCoreContext context)
    {
        Db = context;
        DbSet = Db.Set<Content>();
    }
    public void Add(Content content)
    {
        DbSet.Add(content);
    }

    public async Task<bool> CheckExistById(Guid id)
    {
        return await DbSet.AnyAsync(x => x.Id == id);
    }

    public async Task<(IEnumerable<Content>, int)> Filter(string? keyword, string channel, IFopRequest request)
    {
        var query = DbSet.AsQueryable();
        var channelId = Guid.Empty;
        query = query.Where(x => x.Deleted == null || (x.Deleted.HasValue && !x.Deleted.Value));
        if (!string.IsNullOrEmpty(channel) && Guid.TryParse(channel, out channelId))
        {
            //query = query.Where(x => x.IdGroupCategories.Contains(channelId.ToString()));
            query = query.Where(x => x.ContentGroupCategoryMapping.Select(x => x.GroupCategoryId).Contains(channelId));
        }
        else if (!string.IsNullOrEmpty(channel))
        {
            query = query.Where(x => x.ContentGroupCategoryMapping.Select(x => x.GroupCategory.Code).Contains(channel));
        }


        if (!string.IsNullOrEmpty(keyword))
        {
            keyword = keyword?.KeywordStandardized();
            query = query.Where(x => EF.Functions.Contains(x.Name, $"{keyword}") || x.Code.Contains(keyword));
        }

        var (filtered, totalCount) = query.ApplyFop(request);
        return (await filtered.ToListAsync(), totalCount);
    }

    public async Task<IEnumerable<Content>> Filter(List<string> contents)
    {
        var query = DbSet.AsQueryable();
        return await query.Where(x => contents.Contains(x.Code)).ToListAsync();
    }

    public async Task<IEnumerable<Content>> GetAll()
    {
        return await DbSet.ToListAsync();
    }

    public async Task<Content> GetByCode(string code)
    {
        return await DbSet.FirstOrDefaultAsync(x => x.Code.Equals(code));
    }

    public async Task<Content> GetById(Guid id)
    {
        return await DbSet.FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<IEnumerable<Content>> GetListListBox(Dictionary<string, object> filter, string? keyword)
    {
        var query = DbSet.AsQueryable();
        if (!String.IsNullOrEmpty(keyword))
        {
            query = query.Where(x => x.Name.Contains(keyword));
        }
        foreach (var item in filter)
        {
            if (item.Key.Equals("status"))
            {
                query = query.Where(x => x.Status == Convert.ToInt32(item.Value));
            }
            if (item.Key.Equals("contentCode"))
            {
                query = query.Where(x => ((List<string>)item.Value).Contains(x.Code));
            }
            if (item.Key.Equals("contentId"))
            {
                query = query.Where(x => ((List<Guid>)item.Value).Contains(x.Id));
            }
        }
        return await query.Where(x => x.Deleted == false).ToListAsync();
    }

    public void Remove(Content content)
    {
        DbSet.Remove(content);
    }

    public void Update(Content content)
    {
        DbSet.Update(content);

    }


    public async Task<Content> GetByIdNumber(int idNumber)
    {
        return await DbSet.FirstOrDefaultAsync(x => x.IdNumber.Equals(idNumber));
    }

    public async Task<Content> GetBySlug(string channel, string slug, int? status = null)
    {
        Guid channelId;
        bool isValid = Guid.TryParse(channel, out channelId);

        if (!isValid)
        {
            var groupCategory = Db.Set<GroupCategory>().FirstOrDefault(x => x.Code.Equals(channel));
            if (groupCategory != null)
            {
                channelId = groupCategory.Id;
            }
            else
            {
                return null;
            }
        }

        var query = DbSet.Where(x => x.Slug.Equals(slug) && x.IdGroupCategories.Contains(channelId.ToString()));


        if (status.HasValue)
        {
            query = query.Where(x => x.Status == status);
        }

        return await query.FirstOrDefaultAsync();
    }

    public async Task<Content> GetByCategorySlug(string channel, string category, string slug)
    {

        Guid channelId = Guid.Empty;
        bool isValid = Guid.TryParse(channel, out channelId);
        if (isValid)
        {
            var cateId = Db.Set<Category>().Where(x => x.Slug.Equals(category) && x.GroupCategoryCode.Equals(channel)).FirstOrDefault().Id.ToString();
            return await DbSet.FirstOrDefaultAsync(x => x.Slug.Equals(slug) && x.IdCategories.Contains(cateId) && x.IdGroupCategories.Contains(channelId.ToString()));
        }
        else
        {
            channelId = Db.Set<GroupCategory>().Where(x => x.Code.Equals(channel)).FirstOrDefault().Id;
            var cateId = Db.Set<Category>().Where(x => x.Slug.Equals(category) && x.GroupCategoryCode.Equals(channel)).FirstOrDefault().Id.ToString();
            return await DbSet.FirstOrDefaultAsync(x => x.Slug.Equals(slug) && x.IdCategories.Contains(cateId) && x.IdGroupCategories.Contains(channelId.ToString()));
        }
    }

    public async Task<IEnumerable<Content>> Filter(string? keyword, Dictionary<string, object> filter, int pagesize, int pageindex, int? status = null)
    {
        var query = DbSet.Where(x => !x.Deleted.HasValue || (x.Deleted.HasValue && !x.Deleted.Value));

        if (!string.IsNullOrEmpty(keyword))
        {
            keyword = keyword.KeywordStandardized();
            query = query.Where(x => EF.Functions.Contains(x.Name, keyword) || x.Tags.Contains(keyword));
        }

        // Giữ nguyên điều kiện lọc theo Filter
        foreach (var item in filter)
        {
            switch (item.Key)
            {
                case "channel":
                    var channel = item.Value.ToString();
                    Guid channelId;
                    if (Guid.TryParse(channel, out channelId))
                    {
                        query = query.Where(x => x.IdGroupCategories.Contains(channel));
                    }
                    else
                    {
                        var groupCategory = Db.Set<GroupCategory>().FirstOrDefault(x => x.Code.Equals(channel));
                        if (groupCategory != null)
                        {
                            channelId = groupCategory.Id;
                            query = query.Where(x => x.IdGroupCategories.Contains(channelId.ToString()));
                        }
                    }
                    break;

                case "status":
                    query = query.Where(x => x.Status == Convert.ToInt32(item.Value));
                    break;

                case "categoryId":
                    query = query.Where(x => x.IdCategories.Contains(item.Value.ToString()));
                    break;

                case "categoryCode":
                    var category = Db.Set<Category>().FirstOrDefault(x => x.Code.Equals(item.Value.ToString()));
                    if (category != null)
                    {
                        var cateId = category.Id;
                        query = query.Where(x => x.IdCategories.Contains(cateId.ToString()));
                    }
                    break;
            }
        }

        // Thêm điều kiện status nếu có
        if (status.HasValue)
        {
            query = query.Where(x => x.Status == status.Value);
        }

        // Trả về kết quả
        return await query.OrderByDescending(x => x.CreatedDate)
                          .Skip((pageindex - 1) * pagesize)
                          .Take(pagesize)
                          .ToListAsync();
    }

    public async Task<int> FilterCount(string? keyword, Dictionary<string, object> filter)
    {
        var query = DbSet.AsQueryable();
        query = query.Where(x => x.Deleted == null || (x.Deleted.HasValue && !x.Deleted.Value));
        if (!String.IsNullOrEmpty(keyword))
        {

            keyword = keyword?.KeywordStandardized();
            query = query.Where(x => EF.Functions.Contains(x.Name, $"{keyword}") || x.Tags.Contains(keyword));
            //query = query.Where(x => x.Code.Contains(keyword) || x.Name.Contains(keyword));
        }
        foreach (var item in filter)
        {
            if (item.Key.Equals("channel"))
            {
                var channel = item.Value.ToString();
                Guid channelId = Guid.Empty;
                bool isValid = Guid.TryParse(channel, out channelId);
                if (isValid)
                {
                    query = query.Where(x => x.IdGroupCategories.Contains(channel));
                }
                else
                {
                    channelId = Db.Set<GroupCategory>().Where(x => x.Code.Equals(channel)).FirstOrDefault().Id;
                    query = query.Where(x => x.IdGroupCategories.Contains(channelId.ToString()));
                }

            }
            if (item.Key.Equals("status"))
            {
                query = query.Where(x => x.Status == Convert.ToInt32(item.Value));
            }
            if (item.Key.Equals("categoryId"))
            {
                query = query.Where(x => x.IdCategories.Contains(item.Value.ToString()));
            }
            if (item.Key.Equals("categoryCode"))
            {
                var cateId = Db.Set<Category>().Where(x => x.Code.Equals(item.Value.ToString())).FirstOrDefault().Id;
                query = query.Where(x => x.IdCategories.Contains(cateId.ToString()));
            }

        }
        return await query.CountAsync();
    }
    public async Task<IEnumerable<Content>> Related(string channel, string category, Guid contentId, int max)
    {
        var item = await DbSet.FirstOrDefaultAsync(e => e.Id == contentId).Select(x => new { x.Id, x.IdNumber });
        var query = DbSet.AsQueryable();
        query = query.Where(x => x.Deleted == null || (x.Deleted.HasValue && !x.Deleted.Value));
        query = query.Where(x => x.Status == 1);
        Guid channelId = Guid.Empty;
        bool isValid = Guid.TryParse(channel, out channelId);
        if (isValid)
        {
            query = query.Where(x => x.IdGroupCategories.Contains(channel));
        }
        else
        {
            channelId = Db.Set<GroupCategory>().Where(x => x.Code.Equals(channel)).FirstOrDefault().Id;
            query = query.Where(x => x.IdGroupCategories.Contains(channelId.ToString()));
        }

        Guid categoryId = Guid.Empty;
        if (!string.IsNullOrEmpty(category))
            if (Guid.TryParse(category, out categoryId))
            {
                query = query.Where(x => x.IdCategories.Contains(category));
            }
            else
            {
                categoryId = Db.Set<Category>().Where(x => x.Code.Equals(category) && x.GroupCategoryId.Equals(channelId)).FirstOrDefault().Id;
                query = query.Where(x => x.IdCategories.Contains(categoryId.ToString()));

            }
        var left = await query.Where(x => x.IdNumber < item.IdNumber).OrderByDescending(x => x.IdNumber).Skip(0).Take(max / 2).ToListAsync();
        var lefcount = left.Count;
        var right = await query.Where(x => x.IdNumber > item.IdNumber).OrderBy(x => x.IdNumber).Skip(0).Take(max - lefcount).ToListAsync();
        left.AddRange(right);
        return left;
    }

    public async Task<IEnumerable<Content>> DisplayTop(string channel, string category, int? status, int top)
    {

        var query = DbSet.AsQueryable();
        query = query.Where(x => x.Deleted == null || (x.Deleted.HasValue && !x.Deleted.Value));
        if (status.HasValue)
            query = query.Where(x => x.Status == status.Value);

        Guid channelId = Guid.Empty;
        bool isValid = Guid.TryParse(channel, out channelId);
        if (isValid)
        {
            query = query.Where(x => x.IdGroupCategories.Contains(channel));
        }
        else
        {
            channelId = Db.Set<GroupCategory>().Where(x => x.Code.Equals(channel)).FirstOrDefault().Id;
            query = query.Where(x => x.IdGroupCategories.Contains(channelId.ToString()));
        }

        Guid categoryId = Guid.Empty;
        if (!string.IsNullOrEmpty(category))
            if (Guid.TryParse(category, out categoryId))
            {
                query = query.Where(x => x.IdCategories.Contains(category));
            }
            else
            {
                categoryId = Db.Set<Category>().Where(x => x.Code.Equals(category) && x.GroupCategoryId.Equals(channelId)).FirstOrDefault().Id;
                query = query.Where(x => x.IdCategories.Contains(categoryId.ToString()));

            }

        var result = await query.Where(x => x.DisplayTop.HasValue).OrderBy(x => x.DisplayTop).Skip(0).Take(top).ToListAsync();
        return result;
    }
    public async Task<IEnumerable<Content>> DisplayTop1(string channel, string category, int? status, int top)
    {

        var query = DbSet.AsQueryable();
        query = query.Where(x => x.Deleted == null || (x.Deleted.HasValue && !x.Deleted.Value));
        if (status.HasValue)
            query = query.Where(x => x.Status == status.Value);

        Guid channelId = Guid.Empty;
        bool isValid = Guid.TryParse(channel, out channelId);
        if (isValid)
        {
            query = query.Where(x => x.IdGroupCategories.Contains(channel));
        }
        else
        {
            channelId = Db.Set<GroupCategory>().Where(x => x.Code.Equals(channel)).FirstOrDefault().Id;
            query = query.Where(x => x.IdGroupCategories.Contains(channelId.ToString()));
        }

        Guid categoryId = Guid.Empty;
        if (!string.IsNullOrEmpty(category))
            if (Guid.TryParse(category, out categoryId))
            {
                query = query.Where(x => x.IdCategories.Contains(category));
            }
            else
            {
                categoryId = Db.Set<Category>().Where(x => x.Code.Equals(category) && x.GroupCategoryId.Equals(channelId)).FirstOrDefault().Id;
                query = query.Where(x => x.IdCategories.Contains(categoryId.ToString()));

            }

        var result = await query.Where(x => x.DisplayTop1.HasValue).OrderBy(x => x.DisplayTop1).Skip(0).Take(top).ToListAsync();
        return result;
    }
    public async Task<IEnumerable<Content>> DisplayTop2(string channel, string category, int? status, int top)
    {

        var query = DbSet.AsQueryable();
        query = query.Where(x => x.Deleted == null || (x.Deleted.HasValue && !x.Deleted.Value));
        if (status.HasValue)
            query = query.Where(x => x.Status == status.Value);

        Guid channelId = Guid.Empty;
        bool isValid = Guid.TryParse(channel, out channelId);
        if (isValid)
        {
            query = query.Where(x => x.IdGroupCategories.Contains(channel));
        }
        else
        {
            channelId = Db.Set<GroupCategory>().Where(x => x.Code.Equals(channel)).FirstOrDefault().Id;
            query = query.Where(x => x.IdGroupCategories.Contains(channelId.ToString()));
        }

        Guid categoryId = Guid.Empty;
        if (!string.IsNullOrEmpty(category))
            if (Guid.TryParse(category, out categoryId))
            {
                query = query.Where(x => x.IdCategories.Contains(category));
            }
            else
            {
                categoryId = Db.Set<Category>().Where(x => x.Code.Equals(category) && x.GroupCategoryId.Equals(channelId)).FirstOrDefault().Id;
                query = query.Where(x => x.IdCategories.Contains(categoryId.ToString()));

            }

        var result = await query.Where(x => x.DisplayTop2.HasValue).OrderBy(x => x.DisplayTop2).Skip(0).Take(top).ToListAsync();
        return result;
    }
    public void Dispose()
    {
        Db.Dispose();
    }
}
