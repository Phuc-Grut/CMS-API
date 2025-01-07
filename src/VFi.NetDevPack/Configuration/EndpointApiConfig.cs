using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFi.NetDevPack.Configuration;

public class EndpointApiConfig
{

    public EndpointApiConfig() { }
    public string BaseUrl { get; set; }
    public string AccessToken { get; set; }
    public string JwtKey { get; set; }
    public string JwtIssuer { get; set; }
    public string JwtAudience { get; set; }

}
