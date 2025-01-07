//using VFi.NetDevPack.Configuration;
//using VFi.NetDevPack.Context;
//using Flurl.Http;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net.Http;
//using System.Text;
//using System.Threading.Tasks;

//namespace VFi.NetDevPack.Data
//{
//    public class ApiContext: IDisposable
//    {
//        private readonly IContextUser _context;
//        //protected readonly EndpointApi _endpointApi;
//        protected readonly IFlurlClient _flurlClient;
//        public IFlurlClient HttpClient { get { return _flurlClient; } }
//        private string RequestToken { get; set; }
//        private string BaseUrl { get; set; }
//        public ApiContext(EndpointApi endpointApi,  IContextUser context)
//        {
//            _context = context;
//            _endpointApi = endpointApi;
//            BaseUrl = _endpointApi.Url;
//            _flurlClient = new FlurlClient();
//            _flurlClient.BaseUrl = BaseUrl;
//            if (!string.IsNullOrEmpty(_endpointApi.AccessToken)) 
//                RequestToken = _endpointApi.AccessToken;
//            else
//            {

//            }
//            var temp = _context.Tenant;
//            _flurlClient.WithOAuthBearerToken(RequestToken); 
//        }

//            public void Dispose()
//        {
//            if (_flurlClient != null)
//            {
//                _flurlClient.Dispose();
//            }
//            GC.SuppressFinalize(this);
//        }
//    }
//}
