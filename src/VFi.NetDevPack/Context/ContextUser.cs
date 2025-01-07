using System;
using System.Collections.Generic;
using System.Reflection.PortableExecutable;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

namespace VFi.NetDevPack.Context;

public class ContextUser : IContextUser
{
    private readonly IHttpContextAccessor _accessor;

    public ContextUser(IHttpContextAccessor accessor)
    {
        _accessor = accessor;
    }

    public string? UserName
    {
        get
        {
            return _accessor?.HttpContext?.User?.Identity?.Name;
        }
    }

    public string? FullName
    {
        get
        {
            if (!string.IsNullOrEmpty(UserClaims.FullName))
                return UserClaims.FullName;
            else
                return this.UserName;
        }
    }
    public UserClaims UserClaims
    {
        get
        {
            var result = new UserClaims();
            var claims = GetClaims();
            result.Sub = GetClaimValue(claims, ClaimTypes.NameIdentifier);
            result.Email = GetClaimValue(claims, ClaimTypes.Email);
            result.Username = GetClaimValue(claims, ClaimTypes.Name);
            result.Tenant = GetClaimValue(claims, "tenant");
            result.TenantName = GetClaimValue(claims, "tenant_name");
            result.Data_Zone = GetClaimValue(claims, "data_zone");
            result.Data = GetClaimValue(claims, "data");
            result.FullName = GetClaimValue(claims, "fullname");
            result.Product = GetClaimValue(claims, "product");
            return result;
        }
    }
    public string Tenant
    {
        get
        {
            if (!string.IsNullOrEmpty(this.RuntimeTenant))
                return this.RuntimeTenant;
            return UserClaims.Tenant;
        }
    }
    public string Data_Zone
    {
        get
        {
            if (!string.IsNullOrEmpty(this.RuntimeData_Zone))
                return this.RuntimeData_Zone;
            return UserClaims.Data_Zone;
        }
    }

    public string Data
    {
        get
        {
            if (!string.IsNullOrEmpty(this.RuntimeData))
                return this.RuntimeData;
            return UserClaims.Data;
        }
    }



    private string RuntimeTenant
    {
        get; set;
    }
    private string RuntimeData_Zone
    {
        get; set;
    }
    private string RuntimeData
    {
        get; set;
    }
    public void SetEnvTenant(string tenant, string data_zone, string data)
    {
        this.RuntimeTenant = tenant;
        this.RuntimeData_Zone = data_zone;
        this.RuntimeData = data;
    }
    public Guid GetUserId()
    {
        return IsAutenticated() ? Guid.Parse(_accessor.HttpContext.User.GetUserId()) : Guid.Empty;
    }
    public Guid UserId
    {
        get
        {
            return IsAutenticated() ? Guid.Parse(_accessor.HttpContext.User.GetUserId()) : Guid.Empty;
        }
    }

    public string GetUserEmail()
    {
        return IsAutenticated() ? _accessor.HttpContext.User.GetUserEmail() : "";
    }

    public bool IsAutenticated()
    {

        return _accessor.HttpContext.User.Identity.IsAuthenticated;
    }

    public bool IsInRole(string role)
    {
        return _accessor.HttpContext.User.IsInRole(role);
    }

    public IEnumerable<Claim> GetClaims()
    {
        return _accessor?.HttpContext?.User?.Claims;
    }

    public HttpContext GetHttpContext()
    {
        return _accessor.HttpContext;
    }
    public string? GetToken()
    {
        return _accessor?.HttpContext?.Request.Headers.Authorization.ToString().Replace("Bearer ", "");
    }
    private string GetClaimValue(IEnumerable<Claim> claims, string key)
    {
        try
        {
            return claims.First(x => x.Type == key) != null ? claims.First(x => x.Type == key).Value : "";
        }
        catch (Exception)
        {

            return "";
        }
    }
    public string Product
    {
        get
        {
            var result = new UserClaims();
            var claims = GetClaims();
            return claims.First(x => x.Type.Equals("product")).Value;
        }
    }

}
