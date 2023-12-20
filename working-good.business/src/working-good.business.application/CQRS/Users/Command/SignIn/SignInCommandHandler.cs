using working_good.business.application.CQRS.Abstractions;
using working_good.business.application.Exceptions;
using working_good.business.application.Services;
using working_good.business.core.Abstractions.Repositories;

namespace working_good.business.application.CQRS.Users.Command.SignIn;

internal sealed class SignInCommandHandler(IUserRepository userRepository, IAuthenticator authenticator, IAccessTokenStorage accessTokenStorage) : ICommandHandler<SignInCommand>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IAuthenticator _authenticator = authenticator;
    private readonly IAccessTokenStorage _accessTokenStorage = accessTokenStorage;

    public async Task HandleAsync(SignInCommand command, CancellationToken token)
    {
        var user = await _userRepository.GetByEmailAsync(command.Email);
        if (user is null)
        {
            throw new UserNotFoundException(command.Email, "user_not_found");
        }

        if (!user.CanBeLogged())
        {
            throw new UserCanNotBeLoggedException(command.Email, "user_can_be_logged");
        }
        var accessToken = _authenticator.CreateAccessToken(user.Id, new List<string>() { user.Role });
        _accessTokenStorage.Set(accessToken);
    }
}