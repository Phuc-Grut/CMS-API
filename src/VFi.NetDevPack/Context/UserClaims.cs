namespace VFi.NetDevPack.Context;

public class UserClaims
{
    public int? Exp { get; set; }
    public string Username { get; set; }
    public string Tenant { get; set; }
    public string TenantName { get; set; }
    public string Data_Zone { get; set; }
    public string Data { get; set; }
    public string Email { get; set; }
    public bool Email_Verified { get; set; }
    public string FullName { get; set; }
    public string Sub { get; set; }
    public string[] Roles { get; set; } = new string[0];
    public string Product { get; set; }
}
