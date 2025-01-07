using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VFi.Domain.CMS.Models;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Domain;

namespace VFi.Domain.CMS.Interfaces;

public interface IContentGroupCategoryMappingRepository : IRepository<ContentGroupCategoryMapping>
{
    Task<IEnumerable<ContentGroupCategoryMapping>> Filter(Dictionary<string, object> filter, int pagesize, int pageindex);
    Task<int> FilterCount(Dictionary<string, object> filter);
    Task<IEnumerable<ContentGroupCategoryMapping>> GetListListBox(Dictionary<string, object> filter);
    void Add(IEnumerable<ContentGroupCategoryMapping> items);
    Task<IEnumerable<ContentGroupCategoryMapping>> Filter(Guid Id);
    void Remove(IEnumerable<ContentGroupCategoryMapping> t);
}
