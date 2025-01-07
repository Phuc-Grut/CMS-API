using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VFi.Domain.CMS.Models;
using VFi.NetDevPack.Data;

namespace VFi.Domain.CMS.Interfaces;

public interface IItemRepository : IRepository<Item>
{
    Task<int> FilterCount(string? keyword, Dictionary<string, object> filter);
    Task<IEnumerable<Item>> Filter(string? keyword, Dictionary<string, object> filter, int pagesize, int pageindex);
    Task<List<Item>> GetByProduct(string product);
    Task<IEnumerable<Item>> GetListCbx(Dictionary<string, object> filter);

    Task<Item> GetByPath(string path);

}
