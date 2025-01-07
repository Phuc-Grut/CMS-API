using System.Diagnostics;
using VFi.Infra.CMS.Spider.Interface;
using VFi.NetDevPack.Serialize;
using System.Net;
using System.Net.Http.Headers;
using System.Web;
using System.Net.Sockets;

namespace VFi.Infra.CMS.Spider
{
    public abstract class SpiderClient
    {
        protected readonly IRequestManager requestManager;
        protected SpiderLeg spiderLeg;
        protected HttpClient client => spiderLeg.HttpClient;
        public SpiderClient(IRequestManager requestManager
            ,SpiderLeg spiderLeg
            )
        {
            this.requestManager = requestManager;
            this.spiderLeg = spiderLeg;
        }
         
        protected virtual async Task<TResponse> RetryWithNewProxy<TResponse>(Func<Task<TResponse>> func,
            Func<TResponse, bool> hasNeedToRetry,
            int maxRetry = 5)
        {
            var tempClient = spiderLeg;
            try
            {
                var result = await func.Invoke();

                if (!hasNeedToRetry(result))
                {
                    return result;
                }

                if (maxRetry <= 0)
                {
                    return result;
                }

                await requestManager.Downtime(tempClient.Proxy?.Code);
            }
            catch (Exception ex)
            {
                await requestManager.Downtime(tempClient.Proxy?.Code);
                ex.HelpLink = tempClient.Proxy?.Code;
            }

            spiderLeg = requestManager.GetSpiderLeg();
            return await RetryWithNewProxy(func, hasNeedToRetry, maxRetry - 1);
        }

        protected virtual void ExecconfigHttpRequestMessage(Action<HttpRequestMessage>? configHttpRequestMessage,
            HttpRequestMessage? requestMessage)
        {
            if (configHttpRequestMessage == null
                || requestMessage == null)
            {
                return;
            }

            configHttpRequestMessage.Invoke(requestMessage);
        }

         

        public async Task<(string? httpResponse, string? proxyKey)> GetStringAsync(string uri, HttpMethod method, HttpContent content,
            string? authorizationToken = null,
            string authorizationMethod = "Bearer",
            Action<HttpRequestMessage>? configHttpRequestMessage = null,  CancellationToken cancellationToken = default)
        {
            HttpRequestMessage requestMessage = new(method, uri);
            if(content!=null) requestMessage.Content = content;

            if (authorizationToken != null)
            {
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue(authorizationMethod, authorizationToken);
            }
            ExecconfigHttpRequestMessage(configHttpRequestMessage, requestMessage);
            var response = await spiderLeg.HttpClient.SendAsync(requestMessage,cancellationToken);
            response.EnsureSuccessStatusCode();
            return (await response.Content.ReadAsStringAsync(), spiderLeg.Proxy?.Code);
        }

        

        public SpiderClient SetHeaderForHttpRequestMessage(HttpRequestMessage requestMesage, IDictionary<string, string> headers)
        {
            if (headers == null || headers.Count == 0)
            {
                return this;
            }

            foreach (var item in headers)
            {
                _ = SetHeaderForHttpRequestMessage(requestMesage, item.Key, item.Value);
            }

            return this;
        }

        public SpiderClient SetHeaderForHttpRequestMessage(HttpRequestMessage requestMesage,
            string key,
            string value)
        {
            if (string.IsNullOrWhiteSpace(key) || string.IsNullOrWhiteSpace(value))
            {
                return this;
            }

            if (requestMesage.Headers.Contains(key))
            {
                _ = requestMesage.Headers.Remove(key);
            }

            _ = requestMesage.Headers.TryAddWithoutValidation(key, value);

            return this;
        }
    }
}