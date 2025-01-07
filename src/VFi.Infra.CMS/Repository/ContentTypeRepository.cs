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

public class ContentTypeRepository : IContentTypeRepository
{
    protected readonly SqlCoreContext Db;
    protected readonly DbSet<ContentType> DbSet;
    public IUnitOfWork UnitOfWork => Db;
    public ContentTypeRepository(SqlCoreContext context)
    {
        Db = context;
        DbSet = Db.Set<ContentType>();
    }
    public void Add(ContentType contentType)
    {
        DbSet.Add(contentType);

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

    public async Task<IEnumerable<ContentType>> Filter(string? keyword, Dictionary<string, object> filter, int pagesize, int pageindex)
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
        }
        return await query.OrderBy(x => x.DisplayOrder).Skip((pageindex - 1) * pagesize).Take(pagesize).ToListAsync();
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
        }
        return await query.CountAsync();
    }

    public async Task<IEnumerable<ContentType>> GetAll()
    {
        return await DbSet.ToListAsync();
    }

    public async Task<ContentType> GetById(Guid id)
    {
        return await DbSet.FindAsync(id);
    }

    public async Task<IEnumerable<ContentType>> GetListListBox(int? status, string? keyword)
    {
        var query = DbSet.AsQueryable();
        if (!String.IsNullOrEmpty(keyword))
        {
            query = query.Where(x => x.Code.Contains(keyword) || x.Name.Contains(keyword));
        }
        if (status != null)
        {
            query = query.Where(x => x.Status == status);
        }
        return await query.OrderBy(x => x.DisplayOrder).ToListAsync();
    }

    public void Remove(ContentType contentType)
    {
        DbSet.Remove(contentType);
    }

    public void Update(IEnumerable<ContentType> contentTypes)
    {
        DbSet.UpdateRange(contentTypes);
    }

    public void Update(ContentType contentType)
    {
        DbSet.Update(contentType);
    }

    public async Task<ContentType> GetByCode(string code)
    {
        return await DbSet.FirstOrDefaultAsync(x => x.Code.Equals(code));
    }
}
