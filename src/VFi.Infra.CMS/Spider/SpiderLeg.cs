using VFi.Domain.CMS.Models;
using Flurl.Util;

namespace VFi.Infra.CMS.Spider
{
    public class SpiderLeg
    {
        public Proxy Proxy { get; }
        public HttpClient HttpClient { get; }

        public bool IsActive
        {
            get
            {
                return DownTimeTo < DateTime.Now;
            }
        }

        public DateTime DownTimeTo { get; set; } = DateTime.Now;

        public SpiderLeg(HttpClient httpClient,
            Proxy? proxyInfo)
        {
            HttpClient = httpClient;
            Proxy = proxyInfo;
        }

        public void UnActive(int? seconds)
        {
            DownTimeTo = DateTime.Now.AddSeconds(seconds ?? 300);
        }

        public bool IsEquals(Proxy? proxyInfo)
        {
            if (proxyInfo is null || Proxy is null)
            {
                return false;
            }

            return proxyInfo.Code == Proxy.Code
                && proxyInfo.Host == Proxy.Host
                && proxyInfo.Port == Proxy.Port
                && proxyInfo.UserName == Proxy.UserName
                && proxyInfo.Password == Proxy.Password;
        }
    }
}