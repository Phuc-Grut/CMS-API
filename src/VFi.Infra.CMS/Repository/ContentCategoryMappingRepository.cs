using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VFi.Domain.CMS.Interfaces;
using VFi.Domain.CMS.Models;
using VFi.Infra.CMS.Context;
using VFi.NetDevPack.Data;

namespace VFi.Infra.CMS.Repository;

internal class ContentCategoryMappingRepository : IContentCategoryMappingRepository
{
    protected readonly SqlCoreContext Db;
    protected readonly DbSet<ContentCategoryMapping> DbSet;
    public IUnitOfWork UnitOfWork => Db;
    public ContentCategoryMappingRepository(SqlCoreContext context)
    {
        Db = context;
        DbSet = Db.Set<ContentCategoryMapping>();
    }

    public void Add(IEnumerable<ContentCategoryMapping> contentCategoryMappings)
    {
        DbSet.AddRange(contentCategoryMappings);
    }

    public void Add(ContentCategoryMapping contentCategory)
    {
        DbSet.AddRange(contentCategory);
    }

    public async Task<bool> CheckExistById(Guid id)
    {
        return await DbSet.AnyAsync(x => x.Id == id);
    }

    public void Dispose()
    {
        Db.Dispose();
    }

    public async Task<IEnumerable<ContentCategoryMapping>> Filter(Dictionary<string, object> filter, int pagesize, int pageindex)
    {
        var query = DbSet.AsQueryable();
        foreach (var item in filter)
        {
            if (item.Key.Equals("contentId"))
            {
                query = query.Where(x => x.ContentId.Equals(new Guid(item.Value + "")));
            }
            if (item.Key.Equals("categoryId"))
            {
                query = query.Where(x => x.CategoryId.Equals(new Guid(item.Value + "")));
            }
        }
        return await query.Skip((pageindex - 1) * pagesize).Take(pagesize).ToListAsync();
    }


    public async Task<IEnumerable<ContentCategoryMapping>> Filter(Guid Id)
    {
        return await DbSet.Where(x => x.ContentId == Id).ToListAsync();
    }

    public async Task<int> FilterCount(Dictionary<string, object> filter)
    {
        var query = DbSet.AsQueryable();
        foreach (var item in filter)
        {
            if (item.Key.Equals("contentId"))
            {
                query = query.Where(x => x.ContentId.Equals(new Guid(item.Value + "")));
            }
            if (item.Key.Equals("categoryId"))
            {
                query = query.Where(x => x.CategoryId.Equals(new Guid(item.Value + "")));
            }
        }
        return await query.CountAsync();
    }

    public async Task<IEnumerable<ContentCategoryMapping>> GetAll()
    {
        return await DbSet.ToListAsync();
    }

    public async Task<ContentCategoryMapping> GetById(Guid id)
    {
        return await DbSet.FindAsync(id);
    }

    public async Task<IEnumerable<ContentCategoryMapping>> GetListListBox(Dictionary<string, object> filter)
    {
        var query = DbSet.AsQueryable();
        foreach (var item in filter)
        {
            if (item.Key.Equals("contentId"))
            {
                query = query.Where(x => x.ContentId.Equals(new Guid(item.Value + "")));
            }
            if (item.Key.Equals("categoryId"))
            {
                query = query.Where(x => x.CategoryId.Equals(new Guid(item.Value + "")));
            }
        }
        return await query.OrderBy(x => x.DisplayOrder).ToListAsync();
    }

    public void Remove(IEnumerable<ContentCategoryMapping> t)
    {
        DbSet.RemoveRange(t);
    }

    public void Remove(ContentCategoryMapping contentCategory)
    {
        DbSet.Remove(contentCategory);
    }

    public void Update(ContentCategoryMapping contentCategory)
    {
        DbSet.Update(contentCategory);
    }
}
