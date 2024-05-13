namespace MultiTenant.Domain.Models;

public class Account(string? nome, string username, string token, string role, DateTime createAt, DateTime expires)
{
    public Account(string nome, string username, string token, string role, int expiresInMinutes)
        : this(nome, username, token, role, DateTime.Now, DateTime.Now.AddMinutes(expiresInMinutes))
    {
    }

    public Account()
        : this(string.Empty, string.Empty, string.Empty, string.Empty, 0)
    {
    }

    public string? Nome { get; set; } = nome;
    public string Username { get; set; } = username;
    public string Token { get; set; } = token;
    public string Role { get; set; } = role;
    public DateTime CreatedAt { get; set; } = createAt;
    public DateTime Expires { get; set; } = expires;
}