using VFi.Domain.CMS.Models;
using VFi.NetDevPack.Data;

namespace VFi.Domain.CMS.Interfaces;

public interface ICategoryRepository : IRepository<Category>
{
    Task<Boolean> CheckExist(string? code, Guid? id);
    Task<IEnumerable<Category>> Filter(string? keyword, Dictionary<string, object> filter);
    Task<IEnumerable<Category>> Filter(string? keyword, Dictionary<string, object> filter, int pagesize, int pageindex);
    Task<int> FilterCount(string? keyword, Dictionary<string, object> filter);
    Task<IEnumerable<Category>> GetListListBox(Dictionary<string, object> filter, string? keyword);
    Task<IEnumerable<Category>> GetCombobox(Dictionary<string, object> filter, string? keyword);
    Task<IEnumerable<Category>> GetCombobox(string channel, string? keyword, int? status);
    void Update(IEnumerable<Category> stores);
    Task<IEnumerable<Category>> GetBreadcrumb(string group, string category);
    Task<IEnumerable<Category>> GetByIds(IEnumerable<ContentCategoryMapping> contentCategory);
    Task<Category> GetBySlug(string channel, string slug);
    Task<IEnumerable<Category>> GetCategoriesByListId(IEnumerable<Guid> listId);

}
