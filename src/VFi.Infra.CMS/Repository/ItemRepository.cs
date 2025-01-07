using Microsoft.EntityFrameworkCore;
using VFi.Domain.CMS.Interfaces;
using VFi.Domain.CMS.Models;
using VFi.Infra.CMS.Context;
using VFi.NetDevPack.Data;

namespace VFi.Infra.CMS.Repository;

public class ItemRepository : IItemRepository
{
    protected readonly SqlCoreContext Db;
    protected readonly DbSet<Item> DbSet;
    public IUnitOfWork UnitOfWork => Db;
    public ItemRepository(SqlCoreContext context)
    {
        Db = context;
        DbSet = Db.Set<Item>();
    }

    public void Add(Item item)
    {
        DbSet.Add(item);
    }

    public async Task<bool> CheckExistById(Guid id)
    {
        return await DbSet.AnyAsync(x => x.Id == id);
    }

    public void Dispose()
    {
        Db.Dispose();
    }

    public async Task<IEnumerable<Item>> GetAll()
    {
        return await DbSet.ToListAsync();
    }

    public async Task<Item> GetById(Guid id)
    {
        return await DbSet.FindAsync(id);
    }

    public void Remove(Item item)
    {
        DbSet.Remove(item);
    }

    public void Update(Item item)
    {
        DbSet.Update(item);
    }

    public async Task<int> FilterCount(string? keyword, Dictionary<string, object> filter)
    {
        var query = DbSet.AsQueryable();
        if (!String.IsNullOrEmpty(keyword))
        {
            query = query.Where(x => x.Name.Contains(keyword) || x.Title.Contains(keyword));
        }
        foreach (var item in filter)
        {
            if (item.Key.Equals("status"))
            {
                query = query.Where(x => x.Status == Convert.ToInt32(item.Value));
            }
            if (item.Key.Equals("product"))
            {
                query = query.Where(x => x.Product == Convert.ToString(item.Value));
            }
            if (item.Key.Equals("isFile"))
            {
                query = query.Where(x => x.IsFile == Convert.ToBoolean(item.Value));
            }
            if (item.Key.Equals("parentId"))
            {
                if (item.Value.Equals("null"))
                {
                    query = query.Where(x => x.ParentId == null);
                }
                else
                {
                    query = query.Where(x => x.ParentId.Equals(new Guid(item.Value + "")));
                }

            }
        }
        return await query.CountAsync();
    }

    public async Task<IEnumerable<Item>> Filter(string? keyword, Dictionary<string, object> filter, int pagesize, int pageindex)
    {
        var query = DbSet.AsQueryable();
        if (!String.IsNullOrEmpty(keyword))
        {
            query = query.Where(x => x.Name.Contains(keyword) || x.Title.Contains(keyword));
        }
        foreach (var item in filter)
        {
            if (item.Key.Equals("status"))
            {
                query = query.Where(x => x.Status == Convert.ToInt32(item.Value));
            }
            if (item.Key.Equals("product"))
            {
                query = query.Where(x => x.Product == Convert.ToString(item.Value));
            }
            if (item.Key.Equals("isFile"))
            {
                query = query.Where(x => x.IsFile == Convert.ToBoolean(item.Value));
            }
            if (item.Key.Equals("parentId"))
            {
                if (item.Value.Equals("null"))
                {
                    query = query.Where(x => x.ParentId == null);
                }
                else
                {
                    query = query.Where(x => x.ParentId.Equals(new Guid(item.Value + "")));
                }

            }
        }
        return await query.ToListAsync();
    }

    public async Task<List<Item>> GetByProduct(string product)
    {
        var query = DbSet.AsQueryable();
        return await query.Where(x => product.Contains(x.Product)).ToListAsync();
    }

    public async Task<IEnumerable<Item>> GetListCbx(Dictionary<string, object> filter)
    {
        var query = DbSet.AsQueryable();
        foreach (var item in filter)
        {
            if (item.Key.Equals("parentId"))
            {
                query = query.Where(x => x.ParentId.Equals(new Guid(item.Value + "")));
            }
            if (item.Key.Equals("workspace"))
            {
                query = query.Where(x => x.Workspace.Equals(item.Value + ""));
            }
            if (item.Key.Equals("tenant"))
            {
                query = query.Where(x => x.Tenant.Equals(item.Value + ""));
            }
            if (item.Key.Equals("product"))
            {
                query = query.Where(x => x.Product.Equals(item.Value + ""));
            }
            if (item.Key.Equals("status"))
            {
                query = query.Where(x => x.Status == Convert.ToInt32(item.Value));
            }
        }
        return await query.ToListAsync();
    }

    public async Task<Item> GetByPath(string path)
    {
        Guid itemId = Guid.Empty;
        if (Guid.TryParse(path, out itemId))
        {
            return await DbSet.FindAsync(itemId);
        }
        else
        {
            try
            {
                var item = DbSet.AsQueryable().Where(x => x.VirtualPath.Equals(path)).FirstOrDefault();
                return item;
            }
            catch (Exception)
            {

                var item = DbSet.AsQueryable().Where(x => x.LocalPath.Equals(path)).FirstOrDefault();
                return item;
            }
        }
        throw new NotImplementedException();
    }
}
