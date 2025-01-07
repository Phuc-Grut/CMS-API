using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VFi.Domain.CMS.Models;
using VFi.NetDevPack.Data;

namespace VFi.Domain.CMS.Interfaces;

public interface ICategoryRootRepository : IRepository<CategoryRoot>
{
    Task<Boolean> CheckExist(string? code, Guid? id);
    Task<IEnumerable<CategoryRoot>> Filter(string? keyword, Dictionary<string, object> filter);
    Task<IEnumerable<CategoryRoot>> Filter(string? keyword, Dictionary<string, object> filter, int pagesize, int pageindex);
    Task<int> FilterCount(string? keyword, Dictionary<string, object> filter);
    Task<IEnumerable<CategoryRoot>> GetListListBox(Dictionary<string, object> filter, string? keyword);
    void Update(IEnumerable<CategoryRoot> stores);
    Task<CategoryRoot> GetBySlug(string slug);
}
