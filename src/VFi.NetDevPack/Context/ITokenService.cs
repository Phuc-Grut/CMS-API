
namespace VFi.NetDevPack.Context;

public interface ITokenService
{
    string BuildToken(string key, string issuer, UserClaims user);
    //string GenerateJSONWebToken(string key, string issuer, UserDTO user);
    string BuildToken(string key, string issuer, string audience, UserClaims user);
    bool IsTokenValid(string key, string issuer, string token);
    bool IsTokenValid(string key, string issuer, string audience, string token);
}
