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

public class AccessPermissionRepository : IAccessPermissionRepository
{
    protected readonly SqlCoreContext Db;
    protected readonly DbSet<AccessPermission> DbSet;
    public IUnitOfWork UnitOfWork => Db;
    public AccessPermissionRepository(SqlCoreContext context)
    {
        Db = context;
        DbSet = Db.Set<AccessPermission>();
    }

    public void Add(AccessPermission accessPermission)
    {
        DbSet.Add(accessPermission);
    }

    public async Task<bool> CheckExistById(Guid id)
    {
        return await DbSet.AnyAsync(x => x.Id == id);
    }

    public void Dispose()
    {
        Db.Dispose();
    }

    public async Task<IEnumerable<AccessPermission>> GetAll()
    {
        return await DbSet.ToListAsync();
    }

    public async Task<AccessPermission> GetById(Guid id)
    {
        return await DbSet.FindAsync(id);
    }

    public void Remove(AccessPermission accessPermission)
    {
        DbSet.Remove(accessPermission);
    }

    public void Update(AccessPermission accessPermission)
    {
        DbSet.Update(accessPermission);
    }
}
