namespace MultiTenant.Application.Models;

public class JwtOption
{
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public int ExpireInMinute { get; set; }
    public string Key { get; set; } = Guid.NewGuid().ToString("N");
}
