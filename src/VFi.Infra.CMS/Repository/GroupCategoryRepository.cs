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

public class GroupCategoryRepository : IGroupCategoryRepository
{
    protected readonly SqlCoreContext Db;
    protected readonly DbSet<GroupCategory> DbSet;
    public IUnitOfWork UnitOfWork => Db;
    public GroupCategoryRepository(SqlCoreContext context)
    {
        Db = context;
        DbSet = Db.Set<GroupCategory>();
    }

    public void Add(GroupCategory groupCategory)
    {
        DbSet.Add(groupCategory);
    }

    public void Dispose()
    {
        Db.Dispose();
    }

    public async Task<IEnumerable<GroupCategory>> GetAll()
    {
        return await DbSet.OrderBy(x => x.DisplayOrder).ToListAsync();
    }

    public async Task<GroupCategory> GetById(Guid id)
    {
        return await DbSet.FindAsync(id);
    }
    public async Task<GroupCategory> GetByCode(string code)
    {
        return await DbSet.Where(x => x.Code.Equals(code)).FirstAsync();
    }
    public void Remove(GroupCategory groupCategory)
    {
        DbSet.Remove(groupCategory);
    }

    public void Update(GroupCategory groupCategory)
    {
        DbSet.Update(groupCategory);
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
    public async Task<IEnumerable<GroupCategory>> Filter(string? keyword, Dictionary<string, object> filter, int pagesize, int pageindex)
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

    public async Task<IEnumerable<GroupCategory>> GetListListBox(int? status, string? keyword)
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
    public void Update(IEnumerable<GroupCategory> t)
    {
        DbSet.UpdateRange(t);
    }

    public async Task<IEnumerable<GroupCategory>> GetByListId(IEnumerable<Guid> listId)
    {
        var query = DbSet.AsQueryable();
        return await query.Where(x => listId.Contains(x.Id)).ToListAsync();
    }
}
