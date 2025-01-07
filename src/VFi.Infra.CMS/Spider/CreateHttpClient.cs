
using VFi.Domain.CMS.Models;
using VFi.Infra.CMS.Spider.Interface;
using System.Net;
using System.Net.Sockets;

namespace VFi.Infra.CMS.Spider
{
    public class CreateHttpClient :ICreateHttpClient
    {
        public CreateHttpClient()
        {
        }

        public  string DomainCookie { get; set; } 

        public virtual IDictionary<string, string> DefaultCookies { get; set; } = new Dictionary<string, string>()
        {
        };

        public virtual IDictionary<string, string> DefaultHeaders { get; set; } = new Dictionary<string, string>()
        {
            { "User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/77.0.3865.90 Safari/537.36" },
            { "Accept-Charset", "ISO-8859-1" },
            { "Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3" },
            { "accept-encoding", "gzip, deflate, br" }
        };

        private readonly TimeSpan pooledConnectionLifetime = TimeSpan.FromMinutes(30);
        private CookieContainer cookieContainer = new CookieContainer();

        public HttpClient Create(Proxy? proxyInfo)
        {
            //AppContext.SetSwitch("System.Net.Http.UseSocketsHttpHandler", false);
            if (proxyInfo is null)
            {
                SetDefaultCookies();
                var handler = new SocketsHttpHandler
                {

                    AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip,
                    PooledConnectionLifetime = pooledConnectionLifetime,
                    UseCookies = true,
                    CookieContainer = cookieContainer,
                    ConnectTimeout = TimeSpan.FromSeconds(3)
                };
                //ApplyTo(handler);
                return new HttpClient(handler);
            }
            var schema = proxyInfo.IsSecureConnection ? "https://" : "http://";
            // First create a proxy object
            var proxy = new WebProxy
            {
                Address = new Uri($"{schema}{proxyInfo.Host}:{proxyInfo.Port}"),
                BypassProxyOnLocal = false,
                UseDefaultCredentials = false
            };

            var proxyNeedToAuthen = !string.IsNullOrWhiteSpace(proxyInfo.UserName)
                && !string.IsNullOrWhiteSpace(proxyInfo.Password);

            if (proxyNeedToAuthen)
            {
                var networkCredential = new NetworkCredential(userName: proxyInfo.UserName,
                    password: proxyInfo.Password);

                proxy.Credentials = networkCredential;
            }

            // var handler = new HttpClientHandler
            // {
            //     Proxy = proxy,
            //     AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip,
            //     UseCookies = true,
            //     CookieContainer = cookieContainer
            // };
            //var client = new HttpClient(handler: handler, disposeHandler: true);
            var proxyhandler = new SocketsHttpHandler
            {
                KeepAlivePingDelay = TimeSpan.FromSeconds(60),
                KeepAlivePingTimeout = TimeSpan.FromSeconds(30),
                EnableMultipleHttp2Connections = true,
                AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip,
                PooledConnectionLifetime = pooledConnectionLifetime,
                UseCookies = true,
                CookieContainer = cookieContainer,
                ConnectTimeout = TimeSpan.FromSeconds(3),
                Proxy = proxy,
                UseProxy = true
            };
            var client = new HttpClient(proxyhandler, disposeHandler: true);
            SetDefaultHeader(client);

            return client;
        }

        private void SetDefaultHeader(HttpClient client)
        {
            foreach (var item in DefaultHeaders)
            {
                _ = client.DefaultRequestHeaders.TryAddWithoutValidation(item.Key, item.Value);
            }
        }

        private void SetDefaultCookies()
        {
            foreach (var item in DefaultCookies)
            {
                cookieContainer.Add(new Cookie(item.Key, item.Value, "/")
                {
                    Domain = DomainCookie
                });
            }
        }

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dateTime;
        }
        public  void ApplyTo(SocketsHttpHandler handler)
        {
            CachedAddress cachedAddress = null;
            handler.ConnectCallback = async (context, cancellationToken) =>
            {
                if (cachedAddress == null || cachedAddress.Host != context.DnsEndPoint.Host)
                {
                    // Use DNS to look up the IP address(es) of the target host and filter for IPv4 addresses only
                    IPHostEntry ipHostEntry = await Dns.GetHostEntryAsync(context.DnsEndPoint.Host);
                    IPAddress ipAddress = ipHostEntry.AddressList.FirstOrDefault(i => i.AddressFamily == AddressFamily.InterNetwork);
                    if (ipAddress == null)
                    {
                        cachedAddress = null;
                        throw new Exception($"No IP4 address for {context.DnsEndPoint.Host}");
                    }
                    cachedAddress = new CachedAddress() { Ip = ipAddress, Host = context.DnsEndPoint.Host };
                }

                TcpClient tcp = new();
                await tcp.ConnectAsync(cachedAddress.Ip, context.DnsEndPoint.Port, cancellationToken);
                return tcp.GetStream();
            };
        }

        private class CachedAddress
        {
            public IPAddress Ip;
            public string Host;
        }
    }
}
