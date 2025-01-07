using Microsoft.Extensions.Logging;
using PuppeteerSharp;
using PuppeteerSharp.Mobile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFi.Infra.CMS.Spider
{
    public class MercariToken
    {
        //private IBrowser browser;
        //private IPage page;
        private IList<string> tokenItem = new List<string>();
        private IList<string> tokenRelatedItems = new List<string>();
        private IList<string> tokenHome = new List<string>();
        private IList<string> tokenSearch = new List<string>();
        private IList<string> tokenSeller = new List<string>();
        private readonly ILogger logger;
        public MercariToken(ILogger<RequestManager> logger)
        {
            this.logger = logger;
        }

        public string TokenItem
        {
            get
            {
                if (!tokenItem.Any())
                {
                    Task.Run(ReloadTokenDetail).Wait();
                }
                return tokenItem.Any() ? tokenItem.First() : "";
            }

        }

        public string TokenRelatedItems
        {

            get
            {
                if (!tokenRelatedItems.Any())
                {
                    Task.Run(ReloadTokenDetail).Wait();
                }
                return tokenRelatedItems.Any() ? tokenRelatedItems.First() : "";
            }
        }
        public string TokenHome
        {
            get
            {
                if (!tokenHome.Any())
                {
                    Task.Run(ReloadTokenHome).Wait();
                }
                return tokenHome.Any() ? tokenHome.First() : "";
            }
        }
        public string TokenSellerInfo { 
            get
            {
                if (!tokenSellerInfo.Any())
                {
                    Task.Run(ReloadTokenSeller).Wait();
                }
                return tokenSellerInfo.Any() ? tokenSellerInfo.First() : "";
            }
        }
        public string TokenSearch
        {
            get
            {
                if (!tokenSearch.Any())
                {
                    Task.Run(ReloadTokenSearch).Wait();
                }
                return tokenSearch.Any() ? tokenSearch.First() : "";
            }
        }
        public string TokenSeller
        {
            get
            {
                if (!tokenSeller.Any())
                {
                    Task.Run(ReloadTokenSeller).Wait();
                }
                return tokenSeller.Any() ? tokenSeller.First() : "";
            }
        }
        public bool TokenClear(string token)
        {
            return tokenItem.Remove(token) || tokenRelatedItems.Remove(token) || tokenHome.Remove(token) || tokenSeller.Remove(token) || tokenSellerInfo.Remove(token);
        }
        #region ReloadTokenHome
        private bool IsRunTokenHome = false;
        private async Task ReloadTokenHome()
        {

            try
            {
                tokenHome.Clear();
                if (IsRunTokenHome) return;
                IsRunTokenHome = true;
                var browser = await CreateBrower();
                var page = await browser.NewPageAsync();
                try
                {
                    var isResponse = false;
                    var maxFail = 3;
                    var countFail = 0;
                    page.Response += (obj, requestEventArgs) =>
                    {
                        if (requestEventArgs.Response.Request.Url.Contains("/store/get_items?type=category")
                        && requestEventArgs.Response.Request.Method == HttpMethod.Get)
                        {
                            if (requestEventArgs.Response.Request.Headers.TryGetValue("DPoP", out var token))
                            {
                                if (!string.IsNullOrEmpty(token))
                                {
                                    isResponse = true;
                                    tokenHome.Add(token);
                                }
                            }

                        }

                    };
                    int limit = 1;
                    for (int i = 0; i < limit; i++)
                    {
                        try
                        {
                            isResponse = false;
                            var url = "https://jp.mercari.com";
                            await GotoPage(page, url, null);
                            await WaitFor(() =>
                            {
                                return isResponse;
                            }, 1, 10);
                        }
                        catch (Exception)
                        {
                            if (countFail++ >= maxFail)
                            {
                                break;
                            }
                            i = i - 1;
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, ex.Message);
                }
                finally
                {
                    await CloseBrower(browser, page);
                }
            }
            catch (Exception ex)
            { logger.LogError(ex, ex.Message); }
            IsRunTokenHome = false;
        }
        #endregion
        #region ReloadTokenSearch
        private bool IsRunTokenSearch = false;
        private async Task ReloadTokenSearch()
        {

            try
            {
                tokenSearch.Clear();
                if (IsRunTokenSearch) return;
                IsRunTokenSearch = true;
                var browser = await CreateBrower();
                var page = await browser.NewPageAsync();
                try
                {
                    var isResponse = false;
                    var maxFail = 3;
                    var countFail = 0;
                    page.Response += (obj, requestEventArgs) =>
                    {
                        if (requestEventArgs.Response.Request.Url.Contains("entities:search")
                        && requestEventArgs.Response.Request.Method == HttpMethod.Post)
                        {
                            if (requestEventArgs.Response.Request.Headers.TryGetValue("DPoP", out var token))
                            {
                                if (!string.IsNullOrEmpty(token))
                                {
                                    isResponse = true;
                                    tokenSearch.Add(token);
                                }
                            }

                        }

                    };
                    int limit = 1;
                    for (int i = 0; i < limit; i++)
                    {
                        try
                        {
                            isResponse = false;
                            var url = "https://jp.mercari.com/search?category_id=345";
                            await GotoPage(page, url, null);
                            await WaitFor(() =>
                            {
                                return isResponse;
                            }, 1, 10);
                        }
                        catch (Exception)
                        {
                            if (countFail++ >= maxFail)
                            {
                                break;
                            }
                            i = i - 1;
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, ex.Message);
                }
                finally
                {
                    await CloseBrower(browser, page);
                }
            }
            catch (Exception ex)
            { logger.LogError(ex, ex.Message); }
            IsRunTokenSearch = false;
        }
        #endregion

        #region ReloadTokenDetail
        private bool IsRunTokenDetail = false;
        private async Task ReloadTokenDetail()
        {

            try
            {
                tokenRelatedItems.Clear();
                tokenItem.Clear();
                if (IsRunTokenDetail) return;
                IsRunTokenDetail = true;
                var browser = await CreateBrower();
                var page = await browser.NewPageAsync();
                try
                {
                    var isResponse = false;
                    var isResponseRelated = false;
                    var maxFail = 3;
                    var countFail = 0;
                    page.Response += (obj, requestEventArgs) =>
                    {
                        if (requestEventArgs.Response.Request.Url.Contains("items/get?id=")
                        && requestEventArgs.Response.Request.Method == HttpMethod.Get)
                        {
                            if (requestEventArgs.Response.Request.Headers.TryGetValue("DPoP", out var token))
                            {
                                if (!string.IsNullOrEmpty(token))
                                {
                                    isResponse = true;
                                    tokenItem.Add(token);
                                }
                            }

                        }
                        if (requestEventArgs.Response.Request.Url.Contains("items/related_items?item_id") && requestEventArgs.Response.Request.Method == HttpMethod.Get)
                        {
                            if (requestEventArgs.Response.Request.Headers.TryGetValue("DPoP", out var token))
                            {
                                if (!string.IsNullOrEmpty(token))
                                {
                                    isResponseRelated = true;
                                    tokenRelatedItems.Add(token);
                                }
                            }

                        }

                    };
                    int limit = 1;
                    for (int i = 0; i < limit; i++)
                    {
                        try
                        {
                            isResponse = false;
                            isResponseRelated = false;
                            var url = "https://jp.mercari.com/item/m99531291622";
                            await GotoPage(page, url, null);
                            await WaitFor(() =>
                            {
                                return isResponse && isResponseRelated;
                            }, 1, 10);
                        }
                        catch (Exception)
                        {
                            if (countFail++ >= maxFail)
                            {
                                break;
                            }
                            i = i - 1;
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, ex.Message);
                }
                finally
                {
                    await CloseBrower(browser, page);
                }
            }
            catch (Exception ex)
            { logger.LogError(ex, ex.Message); }
            IsRunTokenDetail = false;
        }
        #endregion


        #region ReloadTokenSeller
        private bool IsRunTokenSellerItems = false;
        private IList<string> tokenSellerInfo = new List<string>();

        private async Task ReloadTokenSeller()
        {

            try
            {
                tokenSeller.Clear(); tokenSellerInfo.Clear();
                if (IsRunTokenSellerItems) return;
                IsRunTokenSellerItems = true;
                var browser = await CreateBrower();
                var page = await browser.NewPageAsync();
                try
                {
                    var isResponse = false;
                    var maxFail = 3;
                    var countFail = 0;
                    page.Response += (obj, requestEventArgs) =>
                    {
                        if (requestEventArgs.Response.Request.Url.Contains("/items/get_items?seller_id")  && requestEventArgs.Response.Request.Method == HttpMethod.Get)
                        {
                            if (requestEventArgs.Response.Request.Headers.TryGetValue("DPoP", out var token))
                            {
                                if (!string.IsNullOrEmpty(token))
                                {
                                    isResponse = true;
                                    tokenSeller.Add(token);
                                }
                            } 
                        }
                        if (requestEventArgs.Response.Request.Url.Contains("get_profile?user_id")  && requestEventArgs.Response.Request.Method == HttpMethod.Get)
                        {
                            if (requestEventArgs.Response.Request.Headers.TryGetValue("DPoP", out var token))
                            {
                                if (!string.IsNullOrEmpty(token))
                                {
                                    isResponse = true;
                                    tokenSellerInfo.Add(token);
                                }
                            }

                        }
                    };
                    int limit = 1;
                    for (int i = 0; i < limit; i++)
                    {
                        try
                        {
                            isResponse = false;
                            var url = "https://jp.mercari.com/user/profile/109287480";
                            await GotoPage(page, url, null);
                            await WaitFor(() =>
                            {
                                return isResponse;
                            }, 1, 10);
                        }
                        catch (Exception)
                        {
                            if (countFail++ >= maxFail)
                            {
                                break;
                            }
                            i = i - 1;
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, ex.Message);
                }
                finally
                {
                    await CloseBrower(browser, page);
                }
            }
            catch (Exception ex)
            { logger.LogError(ex, ex.Message); }
            IsRunTokenSellerItems = false;
        }
        #endregion

        private async Task<IBrowser> CreateBrower()
        {
            var args = new List<string>()
            {
                //"--no-sandbox",
                //"--disable-setuid-sandbox",

                "--disable-gpu",
                "--disable-dev-shm-usage",
                "--disable-setuid-sandbox",
                "--no-first-run",
                "--no-sandbox",
                "--no-zygote"
            };

            //var isHaveProxy = proxyInfo != null
            //    && !string.IsNullOrWhiteSpace(proxyInfo.Host)
            //    && proxyInfo.Port > 0;

            //if (isHaveProxy)
            //{
            //    args.Add($"--proxy-server={proxyInfo.Host}:{proxyInfo.Port}");
            //}

            return await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true,
                Args = args.ToArray()
            });
        }


        private async Task CloseBrower(IBrowser? browser, IPage? page)
        {
            try
            {
                if (browser != null)
                {
                    if (!browser.IsClosed)
                    {
                        await browser.CloseAsync();
                    }

                    browser.Dispose();
                }

                browser = null;

                if (page != null)
                {
                    if (!page.IsClosed)
                    {
                        await page.CloseAsync();
                    }

                    page.Dispose();
                }
                page = null;
            }
            catch
            {
            }
        }
        private async Task GotoPage(IPage page,
           string url,
           DeviceDescriptorName? deviceDescriptorName = DeviceDescriptorName.IPadPro)
        {
            DeviceDescriptor device = null;

            if (deviceDescriptorName != null)
            {
                Puppeteer.Devices.TryGetValue(deviceDescriptorName.Value, out device);
            }
            if (device != null)
            {
                await page.EmulateAsync(device);
            }
            await page.GoToAsync(url, new NavigationOptions()
            {
                WaitUntil = new WaitUntilNavigation[]
                {
                    WaitUntilNavigation.DOMContentLoaded
                }
            });
        }
        protected Task WaitFor(Func<bool> condition, int delaySecond = 1, int maxTotalSecondToWait = 10)
        {
            return WaitFor(() =>
            {
                var result = condition.Invoke();

                return Task.FromResult(result);
            }, delaySecond, maxTotalSecondToWait);
        }
        protected async Task WaitFor(Func<Task<bool>> condition, int delaySecond = 1, int maxTotalSecondToWait = 10)
        {
            for (var i = 0; i < maxTotalSecondToWait; i += delaySecond)
            {
                await Task.Delay(TimeSpan.FromSeconds(delaySecond));

                var isSuccess = await condition.Invoke();
                if (isSuccess)
                {
                    return;
                }
            }

            throw new TimeoutException();
        }
    }
}
