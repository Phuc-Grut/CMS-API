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

public class ItemProductRepository : IItemProductRepository
{
    protected readonly SqlCoreContext Db;
    protected readonly DbSet<ItemProduct> DbSet;
    public IUnitOfWork UnitOfWork => Db;
    public ItemProductRepository(SqlCoreContext context)
    {
        Db = context;
        DbSet = Db.Set<ItemProduct>();
    }
    public void Add(ItemProduct itemProduct)
    {
        DbSet.Add(itemProduct);
    }
    public async Task<bool> CheckExistById(Guid id)
    {
        return await DbSet.AnyAsync(x => x.Id == id);
    }
    public void Dispose()
    {
        Db.Dispose();
    }
    public async Task<IEnumerable<ItemProduct>> GetAll()
    {
        return await DbSet.ToListAsync();
    }
    public async Task<ItemProduct> GetById(Guid id)
    {
        return await DbSet.FindAsync(id);
    }
    public void Remove(ItemProduct itemProduct)
    {
        DbSet.Remove(itemProduct);
    }
    public void Update(ItemProduct itemProduct)
    {
        DbSet.Update(itemProduct);
    }
}
