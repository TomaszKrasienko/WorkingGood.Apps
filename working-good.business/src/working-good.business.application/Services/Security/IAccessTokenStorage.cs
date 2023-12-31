using working_good.business.application.DTOs;

namespace working_good.business.application.Services.Security;

public interface IAccessTokenStorage
{
    void Set(AccessTokenDto accessTokenDto);
    AccessTokenDto? Get();
}