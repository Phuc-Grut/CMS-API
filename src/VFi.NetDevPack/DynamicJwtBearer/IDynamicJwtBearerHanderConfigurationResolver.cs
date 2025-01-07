using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace VFi.NetDevPack.DynamicJwtBearer;

public interface IDynamicJwtBearerHanderConfigurationResolver
{
    Task<OpenIdConnectConfiguration> ResolveCurrentOpenIdConfiguration(HttpContext context);
}
