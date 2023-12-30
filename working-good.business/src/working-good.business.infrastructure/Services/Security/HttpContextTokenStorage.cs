using Microsoft.AspNetCore.Http;
using working_good.business.application.DTOs;
using working_good.business.application.Services;
using working_good.business.application.Services.Security;

namespace working_good.business.infrastructure.Services.Security;

internal sealed class HttpContextTokenStorage(IHttpContextAccessor httpContextAccessor) : IAccessTokenStorage
{
    private const string TokenKey = "jwt";
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public void Set(AccessTokenDto accessTokenDto)
        => _httpContextAccessor.HttpContext?.Items.TryAdd(TokenKey, accessTokenDto);

    public AccessTokenDto Get()
    {
        if (_httpContextAccessor.HttpContext is null)
        {
            return null;
        }

        if (_httpContextAccessor.HttpContext.Items.TryGetValue(TokenKey, out var accessToken))
        {
            return accessToken as AccessTokenDto;
        }

        return null;
    }
}