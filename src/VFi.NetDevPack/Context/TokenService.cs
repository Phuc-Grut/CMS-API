using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace VFi.NetDevPack.Context;

/// <summary>
/// longnd 23/07/09
/// </summary>
public class TokenService : ITokenService
{
    private const double EXPIRY_DURATION_MINUTES = 30;
    public string BuildToken(string key,
    string issuer, UserClaims user)
    {
        var dtDateTime = DateTimeOffset.FromUnixTimeSeconds(user.Exp.Value).LocalDateTime;
        var claims = new[] {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, String.Join(',',user.Roles)),
            new Claim(ClaimTypes.NameIdentifier, user.Sub),
             new Claim(ClaimTypes.Email, user.Email), new Claim("email_verified", user.Email_Verified.ToString()),
             new Claim("tenant", user.Tenant),
             new Claim("tenant_name", user.TenantName),
             new Claim("fullname", user.FullName),
             new Claim("data", user.Data), new Claim("data_zone", user.Data_Zone),
             new Claim("iss", issuer),
             new Claim("product", user.Product)

        };
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
        var tokenDescriptor = new JwtSecurityToken(issuer, "", claims,
            expires: dtDateTime, signingCredentials: credentials);
        return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
    }
    public string BuildToken(string key, string issuer, string audience, UserClaims user)
    {
        var dtDateTime = DateTimeOffset.FromUnixTimeSeconds(user.Exp.Value).LocalDateTime;
        var claims = new[] {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, String.Join(',',user.Roles)),
            new Claim(ClaimTypes.NameIdentifier, user.Sub),
             new Claim(ClaimTypes.Email, user.Email), new Claim("email_verified", user.Email_Verified.ToString()),
             new Claim("tenant", user.Tenant),
             new Claim("tenant_name", user.TenantName),
             new Claim("fullname", user.FullName),
             new Claim("data_zone", user.Data_Zone), new Claim("data_zone", user.Data_Zone),
             new Claim("data", user.Data),
             new Claim("iss", issuer), new Claim("aud",audience),
             new Claim("product", user.Product)

        };

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
        var tokenDescriptor = new JwtSecurityToken(issuer, audience, claims,
            expires: dtDateTime, signingCredentials: credentials);
        return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
    }
    public bool IsTokenValid(string key, string issuer, string token)
    {
        var mySecret = Encoding.UTF8.GetBytes(key);
        var mySecurityKey = new SymmetricSecurityKey(mySecret);

        var tokenHandler = new JwtSecurityTokenHandler();
        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = issuer,
                IssuerSigningKey = mySecurityKey,
            }, out SecurityToken validatedToken);
        }
        catch
        {
            return false;
        }
        return true;
    }
    public bool IsTokenValid(string key, string issuer, string audience, string token)
    {
        var mySecret = Encoding.UTF8.GetBytes(key);
        var mySecurityKey = new SymmetricSecurityKey(mySecret);

        var tokenHandler = new JwtSecurityTokenHandler();
        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = issuer,
                ValidAudience = audience,
                IssuerSigningKey = mySecurityKey,
            }, out SecurityToken validatedToken);
        }
        catch
        {
            return false;
        }
        return true;
    }
}
