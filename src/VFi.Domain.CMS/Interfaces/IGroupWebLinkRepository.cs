using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VFi.Domain.CMS.Models;
using VFi.NetDevPack.Data;

namespace VFi.Domain.CMS.Interfaces;

public interface IGroupWebLinkRepository : IRepository<GroupWebLink>
{
    Task<Boolean> CheckExist(string? code, Guid? id);
    Task<IEnumerable<GroupWebLink>> Filter(string? keyword, Dictionary<string, object> filter, int pagesize, int pageindex);
    Task<int> FilterCount(string? keyword, Dictionary<string, object> filter);
    Task<IEnumerable<GroupWebLink>> GetListListBox(int? status, string? keyword);
    void Update(IEnumerable<GroupWebLink> t);
    Task<GroupWebLink> GetByCode(string code);
}
