using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MultiTenant.Application.Models;
using MultiTenant.Domain.Entities.Core;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MultiTenant.Application.Utils;

public sealed class TokenUtil(JwtOption jwtOptions)
{
    public TokenUtil(IOptions<JwtOption> options)
        : this(options.Value)
    {
    }

    public Tuple<string, int> GenerateToken(User user)
    {
        var expires = DateTime.UtcNow.AddMinutes(jwtOptions.ExpireInMinute);

        var tokenDescriptor = SecurityTokenDescriptor(user,
            expires,
            jwtOptions.Audience,
            jwtOptions.Issuer,
            Encoding.UTF8.GetBytes(jwtOptions.Key));

        var token = GetToken(tokenDescriptor);

        return new Tuple<string, int>(token, jwtOptions.ExpireInMinute);
    }

    private static SecurityTokenDescriptor SecurityTokenDescriptor(User user, DateTime expires, string audience, string issuer, byte[] key)
    {
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("Id", user.Id.ToString("N")),
                new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString("N")),
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim(JwtRegisteredClaimNames.Jti, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Exp, expires.ToString(CultureInfo.InvariantCulture)),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim("Fullname", user.Name ?? string.Empty),
                new Claim(ClaimTypes.Expiration, expires.ToString(CultureInfo.InvariantCulture))
            }),
            Expires = expires,
            Audience = audience,
            Issuer = issuer,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        return tokenDescriptor;
    }

    private static string GetToken(SecurityTokenDescriptor tokenDescriptor)
    {
        var jwtTokenHandler = new JwtSecurityTokenHandler();
        var securityToken = jwtTokenHandler.CreateToken(tokenDescriptor);
        var token = jwtTokenHandler.WriteToken(securityToken);
        return token;
    }
}