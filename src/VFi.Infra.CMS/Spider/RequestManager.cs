using VFi.Domain.CMS.Models;
using VFi.Infra.CMS.Spider.Algorithm;
using VFi.Infra.CMS.Spider.Interface;
using Microsoft.Extensions.Logging;
using PuppeteerSharp;
using PuppeteerSharp.Mobile;
using System.Collections.Generic;

namespace VFi.Infra.CMS.Spider
{
    public class RequestManager : IRequestManager
    {
       
        private IList<SpiderLeg> spiderLegs = new List<SpiderLeg>();
        //mercari
        private MercariToken _mercariToken;

        private readonly ChooseRequestAlgorithm chooseRequestAlgorithm;
        private readonly ILoadProxy reloadProxy;
        private readonly ICreateHttpClient createHttpClient;
        private readonly ILogger logger;
        private const int DOWNTIME_SECONDS = 300;
        private const string AppKey = "VIP";

        public MercariToken MercariToken { get => _mercariToken; }

        public RequestManager(ChooseRequestAlgorithm chooseRequestAlgorithm,
            ILoadProxy reloadProxy,
            ICreateHttpClient createHttpClient, MercariToken mercariToken,
            ILogger<RequestManager> logger)
        {
            this.chooseRequestAlgorithm = chooseRequestAlgorithm;
            this.reloadProxy = reloadProxy;
            this.createHttpClient = createHttpClient;
            this.logger = logger;
            this._mercariToken = mercariToken;
        }
        public List<SpiderLeg> GetSpiderLegs()
        {
            return spiderLegs.ToList();
        }
        public Task Downtime(string? proxyKey)
        {
            if (!string.IsNullOrEmpty(proxyKey))
            {
                var spiderLeg = spiderLegs.FirstOrDefault(item => item.Proxy != null && item.Proxy.Code.Equals(proxyKey));

                if (spiderLeg != null)
                {
                    spiderLeg.UnActive(DOWNTIME_SECONDS);
                    logger.LogDebug("Proxy DownTime Host: {host}", spiderLeg.Proxy.Host);
                }
            }

            return Task.CompletedTask;
        } 
        public SpiderLeg GetSpiderLeg()
        {
            if (spiderLegs is null || !spiderLegs.Any())
            {
                try
                {
                    LoadProxy().Wait();
                }
                catch
                {
                    logger.LogDebug("Using Default Proxy for: {key}", AppKey);
                    return CreateSpiderLeg(null);
                }
            }

            if (spiderLegs is { })
            { 
               // return spiderLegs.Where(x => x.IsActive).OrderBy(item => (new Random()).Next()).First(); 

                var clientsActive = spiderLegs
                 .Where(x => x.IsActive)
                 .ToList();

                var clientHttp = clientsActive.Any()
                    ? chooseRequestAlgorithm.Get(clientsActive)
                    : CreateSpiderLeg(null);
                return clientHttp;
            }
            return CreateSpiderLeg(null);
        }
        private SpiderLeg CreateSpiderLeg(Proxy? proxyInfo)
        {
            var httpClient = createHttpClient.Create(proxyInfo);

            return new SpiderLeg(httpClient, proxyInfo);
        }
        public Task LoadProxy()
        {
            bool isInitialized = false;
            object initLock = new object();
            if (!isInitialized)
            {
                lock (initLock)
                {
                    if (!isInitialized)
                    {
                        var proxies = reloadProxy.Execute(AppKey);
                        foreach (var item in proxies.Result)
                        {
                            logger.LogDebug("Proxy enter for: {AppKey} - {Key} ", AppKey, item.Code);
                            var spiderLeg = CreateSpiderLeg(item);
                            try
                            {
                                var ct = new CancellationTokenSource();
                                ct.CancelAfter(TimeSpan.FromMilliseconds(5000));
                                CancellationToken token = ct.Token;
                                token.ThrowIfCancellationRequested();
                                spiderLegs.Add(spiderLeg);
                            }
                            catch (Exception ex)
                            {
                                //Ignore Error Proxy
                                logger.LogError(ex, ex.Message);
                            }
                        }

                        //load proxy empty. get default client
                        if (!spiderLegs.Any())
                        {
                            logger.LogDebug("Load Proxy Empty, Using Default Proxy for: {key}", AppKey);
                            spiderLegs.Add(CreateSpiderLeg(null));
                        }

                        isInitialized = true;
                    }
                }
            }
            return Task.CompletedTask;
        }

      
        
    }
}