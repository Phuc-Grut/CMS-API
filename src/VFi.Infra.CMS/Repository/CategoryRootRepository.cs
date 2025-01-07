using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VFi.Domain.CMS.Interfaces;
using VFi.Domain.CMS.Models;
using VFi.Infra.CMS.Context;
using VFi.NetDevPack.Data;

namespace VFi.Infra.CMS.Repository;

public class CategoryRootRepository : ICategoryRootRepository
{
    protected readonly SqlCoreContext Db;
    protected readonly DbSet<CategoryRoot> DbSet;
    public IUnitOfWork UnitOfWork => Db;
    public CategoryRootRepository(SqlCoreContext context)
    {
        Db = context;
        DbSet = Db.Set<CategoryRoot>();
    }

    public void Add(CategoryRoot cateRoot)
    {
        DbSet.Add(cateRoot);
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

    public void Dispose()
    {
        Db.Dispose();
    }

    public async Task<IEnumerable<CategoryRoot>> Filter(string? keyword, Dictionary<string, object> filter)
    {
        var query = DbSet.AsQueryable();

        var levelStart = 0;
        var level = 1;
        var levelCount = 1;
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
                query = query.Where(x => x.ParentCategoryId.Equals(parentId));
            }
            else if (levelMax - levelMin > 0)
            {
                query = query.Where(x => x.ParentIds.StartsWith(parentIds));
            }
            query = query.Where(x => x.Level >= levelMin);
            query = query.Where(x => x.Level <= levelMax);
        }


        return await query.OrderBy(x => x.FullName).ToListAsync();
    }

    public async Task<IEnumerable<CategoryRoot>> Filter(string? keyword, Dictionary<string, object> filter, int pagesize, int pageindex)
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
            if (item.Key.Equals("parentId"))
            {
                if (item.Value.Equals("null"))
                {
                    query = query.Where(x => x.ParentCategoryId == null);
                }
                else
                {
                    query = query.Where(x => x.ParentCategoryId.Equals(new Guid(item.Value + "")));
                }

            }
        }
        return await query.OrderBy(x => x.DisplayOrder).ToListAsync();
    }

    public async Task<int> FilterCount(string? keyword, Dictionary<string, object> filter)
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
            if (item.Key.Equals("parentId"))
            {
                if (item.Value.Equals("null"))
                {
                    query = query.Where(x => x.ParentCategoryId == null);
                }
                else
                {
                    query = query.Where(x => x.ParentCategoryId.Equals(new Guid(item.Value + "")));
                }

            }
        }
        return await query.CountAsync();
    }

    public async Task<IEnumerable<CategoryRoot>> GetAll()
    {
        return await DbSet.ToListAsync();
    }

    public async Task<CategoryRoot> GetById(Guid id)
    {
        return await DbSet.FindAsync(id);
    }



    public async Task<IEnumerable<CategoryRoot>> GetListListBox(Dictionary<string, object> filter, string? keyword)
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
            if (item.Key.Equals("parentId"))
            {
                if (item.Value.Equals("null"))
                {
                    query = query.Where(x => x.ParentCategoryId == null);
                }
                else
                {
                    query = query.Where(x => x.ParentCategoryId.Equals(new Guid(item.Value + "")));
                }

            }
        }
        return await query.OrderBy(x => x.DisplayOrder).ToListAsync();
    }

    public void Remove(CategoryRoot cateRoot)
    {
        DbSet.Remove(cateRoot);
    }

    public void Update(IEnumerable<CategoryRoot> stores)
    {
        DbSet.UpdateRange(stores);
    }

    public void Update(CategoryRoot cateRoot)
    {
        DbSet.Update(cateRoot);
    }

    public async Task<CategoryRoot> GetBySlug(string slug)
    {
        return await DbSet.FirstOrDefaultAsync(x => x.Slug.Equals(slug));
    }
}
