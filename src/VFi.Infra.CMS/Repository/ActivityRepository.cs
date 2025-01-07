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

public class ActivityRepository : IActivityRepository
{
    protected readonly SqlCoreContext Db;
    protected readonly DbSet<Activity> DbSet;
    public IUnitOfWork UnitOfWork => Db;
    public ActivityRepository(SqlCoreContext context)
    {
        Db = context;
        DbSet = Db.Set<Activity>();
    }

    public void Add(Activity accessPermission)
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

    public async Task<IEnumerable<Activity>> GetAll()
    {
        return await DbSet.ToListAsync();
    }

    public async Task<Activity> GetById(Guid id)
    {
        return await DbSet.FindAsync(id);
    }

    public void Remove(Activity activity)
    {
        DbSet.Remove(activity);
    }

    public void Update(Activity activity)
    {
        DbSet.Update(activity);
    }
}
