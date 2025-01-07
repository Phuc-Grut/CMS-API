using VFi.Domain.CMS.Models;
namespace VFi.Infra.CMS.Spider.Interface
{
    public interface ICreateHttpClient
    {
        HttpClient Create(Proxy? proxyInfo);
    }
}