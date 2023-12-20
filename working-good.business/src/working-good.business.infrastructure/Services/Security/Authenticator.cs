using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using working_good.business.application.DTOs;
using working_good.business.application.Services;

namespace working_good.business.infrastructure.Services.Security;

internal sealed class Authenticator : IAuthenticator
{
    private readonly JwtSecurityTokenHandler _tokenHandler = new JwtSecurityTokenHandler();
    private readonly TimeSpan _expiry;
    private readonly string _issuer;
    private readonly string _audience;
    private readonly SigningCredentials _signingCredentials;
    public AccessTokenDto CreateAccessToken(Guid userId, List<string> roles)
    {
        var now = DateTime.Now;
        var expiredDateTime = now.Add(_expiry);
        var claims = new List<Claim>()
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, userId.ToString()),
        };
        roles.ForEach(x => claims.Add(new Claim(ClaimTypes.Role, x)));

        var jwtToken = new JwtSecurityToken(_issuer, _audience, claims, now, expiredDateTime, _signingCredentials);
        return new AccessTokenDto(_tokenHandler.WriteToken(jwtToken));
    }
}