using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VFi.Domain.CMS.Models;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.CMS.Interfaces;

public interface IContentRepository : IRepository<Content>
{
    Task<Content> GetByCategorySlug(string channel, string category, string slug);
    Task<Content> GetByCode(string code);
    Task<(IEnumerable<Content>, int)> Filter(string? keyword, string channel, IFopRequest request);
    Task<IEnumerable<Content>> GetListListBox(Dictionary<string, object> filter, string? keyword);
    Task<IEnumerable<Content>> Filter(List<string> request);
    Task<Content> GetBySlug(string channel, string slug, int? status = null);
    Task<Content> GetByIdNumber(int idNumber);
    Task<IEnumerable<Content>> Filter(string? keyword, Dictionary<string, object> filter, int pagesize, int pageindex, int? status = null);
    Task<int> FilterCount(string? keyword, Dictionary<string, object> filter);
    Task<IEnumerable<Content>> Related(string channel, string category, Guid contentId, int max);
    Task<IEnumerable<Content>> DisplayTop(string channel, string category, int? status, int top);
    Task<IEnumerable<Content>> DisplayTop1(string channel, string category, int? status, int top);
    Task<IEnumerable<Content>> DisplayTop2(string channel, string category, int? status, int top);
}
