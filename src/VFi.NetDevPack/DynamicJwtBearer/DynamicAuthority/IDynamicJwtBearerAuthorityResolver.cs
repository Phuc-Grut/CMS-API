using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace VFi.NetDevPack.DynamicJwtBearer.DynamicAuthority;

public interface IDynamicJwtBearerAuthorityResolver
{
    public TimeSpan ExpirationOfConfiguration { get; }

    public Task<string> ResolveAuthority(HttpContext httpContext);
}
