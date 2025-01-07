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

public class OpenActivityRepository : IOpenActivityRepository
{
    protected readonly SqlCoreContext Db;
    protected readonly DbSet<OpenActivity> DbSet;
    public IUnitOfWork UnitOfWork => Db;
    public OpenActivityRepository(SqlCoreContext context)
    {
        Db = context;
        DbSet = Db.Set<OpenActivity>();
    }

    public void Add(OpenActivity openActivity)
    {
        DbSet.Add(openActivity);
    }

    public async Task<bool> CheckExistById(Guid id)
    {
        return await DbSet.AnyAsync(x => x.Id == id);
    }

    public void Dispose()
    {
        Db.Dispose();
    }

    public async Task<IEnumerable<OpenActivity>> GetAll()
    {
        return await DbSet.ToListAsync();
    }

    public async Task<OpenActivity> GetById(Guid id)
    {
        return await DbSet.FindAsync(id);
    }

    public void Remove(OpenActivity openActivity)
    {
        DbSet.Remove(openActivity);
    }

    public void Update(OpenActivity openActivity)
    {
        DbSet.Update(openActivity);
    }
}
