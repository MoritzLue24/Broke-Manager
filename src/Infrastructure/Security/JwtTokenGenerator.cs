using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Common.Interfaces.Security;
using Domain.Enums;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Security;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly JwtSettings _jwtSettings;

    public JwtTokenGenerator(IOptions<JwtSettings> jwtSettings)
    {
        this._jwtSettings = jwtSettings.Value;
    }

    public string GenToken(Guid userId, IEnumerable<Role> roles)
    {
        // Specify the signing credentials
        // SecurityKey has to match the one specified in DependencyInjection.cs
        var signingCreds = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this._jwtSettings.Secret)),
            SecurityAlgorithms.HmacSha256
        );

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userId.ToString()),
        };

        foreach (Role role in roles)
            claims.Add(new Claim(ClaimTypes.Role, role.ToString()));

        var token = new JwtSecurityToken(
            issuer: this._jwtSettings.Issuer,
            audience: this._jwtSettings.Audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(this._jwtSettings.ExpiryMinutes),
            signingCredentials: signingCreds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
