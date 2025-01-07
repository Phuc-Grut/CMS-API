using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VFi.Domain.CMS.Interfaces;
using VFi.Domain.CMS.Models;
using VFi.Infra.CMS.Context;
using VFi.NetDevPack.Data;

namespace VFi.Infra.CMS.Repository;

public class ContentGroupCategoryMappingRepository : IContentGroupCategoryMappingRepository
{
    protected readonly SqlCoreContext Db;
    protected readonly DbSet<ContentGroupCategoryMapping> DbSet;
    public IUnitOfWork UnitOfWork => Db;
    public ContentGroupCategoryMappingRepository(SqlCoreContext context)
    {
        Db = context;
        DbSet = Db.Set<ContentGroupCategoryMapping>();
    }

    public void Add(ContentGroupCategoryMapping contentAttributeOption)
    {
        DbSet.Add(contentAttributeOption);
    }
    public void Add(IEnumerable<ContentGroupCategoryMapping> contentAttributeOption)
    {
        DbSet.AddRange(contentAttributeOption);
    }
    public void Dispose()
    {
        Db.Dispose();
    }
    public async Task<IEnumerable<ContentGroupCategoryMapping>> Filter(Dictionary<string, object> filter, int pagesize, int pageindex)
    {
        var query = DbSet.AsQueryable();
        foreach (var item in filter)
        {
            if (item.Key.Equals("ContentId"))
            {
                query = query.Where(x => x.ContentId.Equals(new Guid(item.Value + "")));
            }
            if (item.Key.Equals("groupCategoryId"))
            {
                query = query.Where(x => x.GroupCategoryId.Equals(new Guid(item.Value + "")));
            }

        }
        return await query.Skip((pageindex - 1) * pagesize).Take(pagesize).ToListAsync();
    }

    public async Task<int> FilterCount(Dictionary<string, object> filter)
    {
        var query = DbSet.AsQueryable();
        foreach (var item in filter)
        {
            if (item.Key.Equals("ContentId"))
            {
                query = query.Where(x => x.ContentId.Equals(new Guid(item.Value + "")));
            }
            if (item.Key.Equals("groupCategoryId"))
            {
                query = query.Where(x => x.GroupCategoryId.Equals(new Guid(item.Value + "")));
            }
        }
        return await query.CountAsync();
    }

    public async Task<IEnumerable<ContentGroupCategoryMapping>> GetAll()
    {
        return await DbSet.ToListAsync();
    }

    public async Task<ContentGroupCategoryMapping> GetById(Guid id)
    {
        return await DbSet.FindAsync(id);
    }

    public void Remove(ContentGroupCategoryMapping contentAttributeOption)
    {
        DbSet.Remove(contentAttributeOption);
    }

    public void Update(ContentGroupCategoryMapping contentAttributeOption)
    {
        DbSet.Update(contentAttributeOption);
    }

    public async Task<IEnumerable<ContentGroupCategoryMapping>> GetListListBox(Dictionary<string, object> filter)
    {
        var query = DbSet.AsQueryable();
        foreach (var item in filter)
        {
            if (item.Key.Equals("contentId"))
            {
                query = query.Where(x => x.ContentId.Equals(new Guid(item.Value + "")));
            }
            if (item.Key.Equals("groupCategoryId"))
            {
                query = query.Where(x => x.GroupCategoryId.Equals(new Guid(item.Value + "")));
            }
            //if (item.Key.Equals("status"))
            //{
            //    query = query.Where(x => (x.status == 1));
            //}
        }
        return await query.ToListAsync();
    }
    public async Task<bool> CheckExistById(Guid id)
    {
        return await DbSet.AnyAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<ContentGroupCategoryMapping>> Filter(Guid Id)
    {
        return await DbSet.Where(x => x.ContentId == Id).ToListAsync();
    }

    public void Remove(IEnumerable<ContentGroupCategoryMapping> t)
    {
        DbSet.RemoveRange(t);
    }
}
