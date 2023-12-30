using working_good.business.application.DTOs;

namespace working_good.business.application.Services.Security;

public interface IAuthenticator
{
    AccessTokenDto CreateAccessToken(Guid userId, List<string> roles);
}