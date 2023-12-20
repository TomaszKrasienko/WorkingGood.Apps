using Microsoft.AspNetCore.Http;
using working_good.business.application.DTOs;
using working_good.business.application.Services;

namespace working_good.business.infrastructure.Services.Security;

internal sealed class HttpContextTokenStorage(IHttpContextAccessor httpContextAccessor) : IAccessTokenStorage
{
    private const string TokenKey = "jwt";
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public void Set(AccessTokenDto accessTokenDto)
        => _httpContextAccessor.HttpContext?.Items.TryAdd(accessTokenDto, TokenKey);
}