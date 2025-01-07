
using VFi.Domain.CMS.Interfaces;
using VFi.Domain.CMS.Models;
using VFi.Infra.CMS.Context;
using VFi.Infra.CMS.Spider.Interface;
using Microsoft.Extensions.Logging;

namespace VFi.Infra.CMS.Spider
{
    public class LoadProxy : ILoadProxy
    {
        private ILogger logger;
        private IMasterRepository _masterRepository;

        public LoadProxy(ILogger<LoadProxy> logger, IMasterRepository masterRepository)
        {
            this.logger = logger;
            _masterRepository = masterRepository;
        }

        public async Task<IReadOnlyCollection<Proxy>> Execute(string appKey)
        {
            try
            {
                var response = await _masterRepository.GetList(appKey);
                return response.Select(item => new Proxy()
                {
                    Code = item.Code,
                    Host = item.Host,
                    Port = item.Port,
                    UserName = item.UserName,
                    Password = item.Password,
                    Status = item.Status,
                    GroupName = item.GroupName
                }).ToList();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
            }
            return new List<Proxy>();
        }
    }
}