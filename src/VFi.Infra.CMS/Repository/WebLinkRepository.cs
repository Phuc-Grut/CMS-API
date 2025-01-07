using System.Text.RegularExpressions;
using Consul.Filtering;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using VFi.Domain.CMS.Models;
using VFi.Infra.CMS.Context;
using VFi.NetDevPack.Data;

namespace VFi.Domain.CMS.Interfaces;

public class WebLinkRepository : IWebLinkRepository
{
    protected readonly SqlCoreContext Db;
    protected readonly DbSet<WebLink> DbSet;
    public IUnitOfWork UnitOfWork => Db;
    public WebLinkRepository(SqlCoreContext context)
    {
        Db = context;
        DbSet = Db.Set<WebLink>();
    }

    public void Add(WebLink WebLink)
    {
        DbSet.Add(WebLink);
    }

    public void Dispose()
    {
        Db.Dispose();
    }

    public async Task<IEnumerable<WebLink>> GetAll()
    {
        return await DbSet.ToListAsync();
    }

    public async Task<WebLink> GetById(Guid id)
    {
        return await DbSet.FindAsync(id);
    }

    public void Remove(WebLink WebLink)
    {

        DbSet.Remove(WebLink);
    }

    public void Update(WebLink WebLink)
    {

        DbSet.Update(WebLink);
    }
    public void Update(IEnumerable<WebLink> stores)
    {

        DbSet.UpdateRange(stores);
    }
    public async Task<bool> CheckExist(string? code, Guid? id)
    {
        if (id == null)
        {
            if (String.IsNullOrEmpty(code))
            {
                return false;
            }
            return await DbSet.AnyAsync(x => x.Code.Equals(code));
        }
        return await DbSet.AnyAsync(x => x.Code.Equals(code) && x.Id != id);
    }
    public async Task<bool> CheckExistById(Guid id)
    {
        return await DbSet.AnyAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<WebLink>> Filter(string? keyword, Dictionary<string, object> filter, int pagesize, int pageindex)
    {
        var query = DbSet.AsQueryable();
        if (!String.IsNullOrEmpty(keyword))
        {
            query = query.Where(x => (x.Code.Equals(keyword) || x.Name.Contains(keyword) || x.FullName.Contains(keyword) || x.Keywords.Contains(keyword)));
        }
        foreach (var item in filter)
        {
            if (item.Key.Equals("status"))
            {
                query = query.Where(x => x.Status == Convert.ToInt32(item.Value));
            }
            if (item.Key.Equals("groupId"))
            {
                query = query.Where(x => x.GroupWebLinkId.Equals(new Guid(item.Value + "")));
            }
            if (item.Key.Equals("group"))
            {
                query = query.Where(x => x.GroupWebLinkCode.Equals(item.Value + ""));
            }
            if (item.Key.Equals("parentId"))
            {
                if (item.Value.Equals("null"))
                {
                    query = query.Where(x => x.ParentWebLinkId == null);
                }
                else
                {
                    query = query.Where(x => x.ParentWebLinkId.Equals(new Guid(item.Value + "")));
                }

            }
        }
        //var sqlString = query.ToString();
        return await query.OrderBy(x => x.DisplayOrder).ToListAsync();
    }

    public async Task<int> FilterCount(string? keyword, Dictionary<string, object> filter)
    {
        var query = DbSet.AsQueryable();
        if (!String.IsNullOrEmpty(keyword))
        {
            query = query.Where(x => (x.Code.Equals(keyword) || x.Name.Contains(keyword) || x.FullName.Contains(keyword) || x.Keywords.Contains(keyword)));
        }
        foreach (var item in filter)
        {
            if (item.Key.Equals("status"))
            {
                query = query.Where(x => x.Status == Convert.ToInt32(item.Value));
            }
            if (item.Key.Equals("groupId"))
            {
                query = query.Where(x => x.GroupWebLinkId.Equals(new Guid(item.Value + "")));
            }
            if (item.Key.Equals("group"))
            {
                query = query.Where(x => x.GroupWebLinkCode.Equals(item.Value + ""));
            }
            if (item.Key.Equals("parentId"))
            {
                if (item.Value.Equals("null"))
                {
                    query = query.Where(x => x.ParentWebLinkId == null);
                }
                else
                {
                    query = query.Where(x => x.ParentWebLinkId.Equals(new Guid(item.Value + "")));
                }

            }
        }
        return await query.CountAsync();
    }

    public async Task<IEnumerable<WebLink>> GetListListBox(Dictionary<string, object> filter, string? keyword)
    {
        var query = DbSet.AsQueryable();
        if (!String.IsNullOrEmpty(keyword))
        {
            query = query.Where(x => x.Code.Contains(keyword) || x.Name.Contains(keyword));
        }
        foreach (var item in filter)
        {
            if (item.Key.Equals("status"))
            {
                query = query.Where(x => x.Status == Convert.ToInt32(item.Value));
            }
            if (item.Key.Equals("groupId"))
            {
                query = query.Where(x => x.GroupWebLinkId.Equals(new Guid(item.Value + "")));
            }
            if (item.Key.Equals("parentId"))
            {
                if (item.Value.Equals("null"))
                {
                    query = query.Where(x => x.ParentWebLinkId == null);
                }
                else
                {
                    query = query.Where(x => x.ParentWebLinkId.Equals(new Guid(item.Value + "")));
                }

            }
        }
        return await query.OrderBy(x => x.DisplayOrder).ToListAsync();

    }
    public async Task<IEnumerable<WebLink>> GetCombobox(Dictionary<string, object> filter, string? keyword)
    {
        var query = DbSet.AsQueryable();
        query = query.Where(x => (keyword == null || keyword == "" || x.Name.Contains(keyword)) && x.Status == 1);

        foreach (var item in filter)
        {
            if (item.Key.Equals("groupId"))
            {
                var ListGroupWebLinkIds = item.Value.ToString().Split(',').ToList();
                query = query.Where(x => ListGroupWebLinkIds.Contains(x.GroupWebLinkId.ToString()) || item.Value.Equals("null"));
            }
        }
        return await query.OrderBy(x => x.FullName).Skip(0).Take(100).ToListAsync();
    }


    public async Task<IEnumerable<WebLink>> Filter(string? keyword, Dictionary<string, object> filter)
    {
        var query = DbSet.AsQueryable();

        var levelStart = 0;
        var level = 1;
        var levelCount = 1;
        var top = 0;
        Guid parentId = Guid.Empty;
        var levelMin = 0;
        var levelMax = 0;
        string parentIds = "";
        if (!String.IsNullOrEmpty(keyword))
        {
            query = query.Where(x => x.Code.Contains(keyword) || x.Name.Contains(keyword));
        }
        foreach (var item in filter)
        {
            if (item.Key.Equals("status"))
            {
                if (string.IsNullOrEmpty(item.Value.ToString()))
                    continue;
                query = query.Where(x => x.Status == Convert.ToInt32(item.Value));
                continue;
            }
            if (item.Key.Equals("group"))
            {
                query = query.Where(x => x.GroupWebLinkCode.Equals(item.Value.ToString()));
                continue;
            }
            if (item.Key.Equals("groupid"))
            {
                query = query.Where(x => x.GroupWebLinkId.Equals(Guid.Parse(item.Value.ToString())));
                continue;
            }
            if (item.Key.Equals("parent"))
            {
                if (item.Value == null || string.IsNullOrEmpty(item.Value.ToString()))
                    continue;
                levelStart = 1;

                if (!Guid.TryParse(item.Value.ToString(), out parentId))
                {
                    try
                    {
                        var cate = DbSet.Where(x => x.Code.Equals(item.Value.ToString())).Select(x => new { x.Id, x.Level, x.ParentIds }).First();
                        parentId = cate.Id;
                        levelStart = cate.Level.HasValue ? cate.Level.Value + 1 : 1;
                        parentIds = cate.ParentIds;
                    }
                    catch (Exception)
                    { }
                }
                else
                {
                    try
                    {
                        var cate = DbSet.FindAsync(parentId).Result;
                        parentId = cate.Id;
                        parentIds = cate.ParentIds;
                        levelStart = cate.Level.HasValue ? cate.Level.Value + 1 : 1;
                    }
                    catch (Exception)
                    { }
                }

                continue;

            }
            if (item.Key.Equals("level"))
            {

                if (item.Value != null)
                {
                    level = Convert.ToInt32(item.Value);
                }
                levelMin = level - 1 + levelStart;

                continue;
            }
            if (item.Key.Equals("levelCount"))
            {

                if (item.Value != null)
                {
                    levelCount = Convert.ToInt32(item.Value);
                }
                levelMax = level - 1 + levelStart + levelCount - 1;

                continue;
            }
            if (item.Key.Equals("top"))
            {
                top = Int32.Parse(item.Value.ToString());
                continue;
            }
        }
        if (parentId.Equals(Guid.Empty))
        {
            query = query.Where(x => x.Level >= levelMin);
            query = query.Where(x => x.Level <= levelMax);
        }
        else
        {
            if (levelMin == levelMax)
            {
                query = query.Where(x => x.ParentWebLinkId.Equals(parentId));
            }
            else if (levelMax - levelMin > 0)
            {
                query = query.Where(x => x.ParentIds.StartsWith(parentIds));
            }
            query = query.Where(x => x.Level >= levelMin);
            query = query.Where(x => x.Level <= levelMax);
        }
        if (top > 0)
            return await query.OrderBy(x => x.DisplayOrder).Skip(0).Take(top).ToListAsync();
        return await query.OrderBy(x => x.DisplayOrder).ToListAsync();

    }
    public async Task<IEnumerable<WebLink>> GetBreadcrumb(string group, string WebLink)
    {
        var query = DbSet.AsQueryable();
        Guid cateId = Guid.Empty;
        string parentIds = "";

        if (!Guid.TryParse(WebLink, out cateId))
        {
            try
            {
                var cate = DbSet.Where(x => x.Code.Equals(WebLink) && x.GroupWebLinkCode.Equals(group)).Select(x => new { x.ParentIds }).First();
                parentIds = cate.ParentIds;
            }
            catch (Exception)
            { }
        }
        else
        {
            try
            {
                var cate = DbSet.FindAsync(cateId).Result;
                parentIds = cate.ParentIds;
            }
            catch (Exception)
            { }
        }
        var listParentId = parentIds.Split(',').Select(x => new Guid(x));
        query = query.Where(x => listParentId.Contains(x.Id) && x.GroupWebLinkCode.Equals(group));

        return await query.OrderBy(x => x.Level).ToListAsync();

    }

    public async Task<IEnumerable<WebLink>> GetByGroup(List<string> groups, int? status)
    {
        var query = DbSet.AsQueryable();
        if (status.HasValue)
        {
            query = query.Where(x => x.Status == status.Value);
        }
        if (groups.Any())
        {
            query = query.Where(x => groups.Contains(x.GroupWebLinkCode));
        }
        return await query.ToListAsync();
    }
}
