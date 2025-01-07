
using VFi.Domain.CMS.Models;

namespace VFi.Infra.CMS.Spider.Interface
{
    public interface ILoadProxy
    {
        /// <summary>
        /// Execute loading proxy info models method
        /// </summary>
        /// 
        /// <remarks>
        /// Execute loading proxy info models method. From DB or Cache,...
        /// </remarks>
        Task<IReadOnlyCollection<Proxy>> Execute(string appKey);
    }
}