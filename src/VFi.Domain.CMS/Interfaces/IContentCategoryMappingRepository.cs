using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VFi.Domain.CMS.Models;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Domain;

namespace VFi.Domain.CMS.Interfaces;

public interface IContentCategoryMappingRepository : IRepository<ContentCategoryMapping>
{
    Task<IEnumerable<ContentCategoryMapping>> Filter(Dictionary<string, object> filter, int pagesize, int pageindex);
    Task<int> FilterCount(Dictionary<string, object> filter);
    Task<IEnumerable<ContentCategoryMapping>> GetListListBox(Dictionary<string, object> filter);
    void Add(IEnumerable<ContentCategoryMapping> items);
    Task<IEnumerable<ContentCategoryMapping>> Filter(Guid Id);
    void Remove(IEnumerable<ContentCategoryMapping> t);
}
