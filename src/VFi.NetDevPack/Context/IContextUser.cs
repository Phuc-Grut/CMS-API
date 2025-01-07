using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace VFi.NetDevPack.Context;

public interface IContextUser
{
    string Product { get; }
    UserClaims UserClaims { get; }
    string UserName { get; }
    string FullName { get; }
    Guid UserId { get; }
    string Tenant { get; }
    string Data_Zone { get; }
    string Data { get; }
    Guid GetUserId();
    string GetUserEmail();
    bool IsAutenticated();
    bool IsInRole(string role);
    IEnumerable<Claim> GetClaims();
    HttpContext GetHttpContext();
    string GetToken();

    void SetEnvTenant(string tenant, string data_zone, string data);

}
