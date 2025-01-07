using VFi.Domain.CMS.Models;
using VFi.NetDevPack.Data;

namespace VFi.Domain.CMS.Interfaces;

public interface IWebLinkRepository : IRepository<WebLink>
{
    Task<Boolean> CheckExist(string? code, Guid? id);
    Task<IEnumerable<WebLink>> Filter(string? keyword, Dictionary<string, object> filter);
    Task<IEnumerable<WebLink>> Filter(string? keyword, Dictionary<string, object> filter, int pagesize, int pageindex);
    Task<int> FilterCount(string? keyword, Dictionary<string, object> filter);
    Task<IEnumerable<WebLink>> GetListListBox(Dictionary<string, object> filter, string? keyword);
    Task<IEnumerable<WebLink>> GetCombobox(Dictionary<string, object> filter, string? keyword);
    void Update(IEnumerable<WebLink> stores);
    Task<IEnumerable<WebLink>> GetBreadcrumb(string group, string WebLink);
    Task<IEnumerable<WebLink>> GetByGroup(List<string> groups, int? status);

}
