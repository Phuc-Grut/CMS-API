using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VFi.Domain.CMS.Models;
using VFi.NetDevPack.Data;

namespace VFi.Domain.CMS.Interfaces;

public interface IGroupCategoryRepository : IRepository<GroupCategory>
{
    Task<Boolean> CheckExist(string? code, Guid? id);
    Task<IEnumerable<GroupCategory>> Filter(string? keyword, Dictionary<string, object> filter, int pagesize, int pageindex);
    Task<int> FilterCount(string? keyword, Dictionary<string, object> filter);
    Task<IEnumerable<GroupCategory>> GetListListBox(int? status, string? keyword);
    void Update(IEnumerable<GroupCategory> t);
    Task<GroupCategory> GetByCode(string code);
    Task<IEnumerable<GroupCategory>> GetByListId(IEnumerable<Guid> listId);
}
