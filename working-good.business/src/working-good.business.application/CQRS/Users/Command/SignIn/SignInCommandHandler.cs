using working_good.business.application.CQRS.Abstractions;
using working_good.business.application.Exceptions;
using working_good.business.application.Services;
using working_good.business.core.Abstractions;
using working_good.business.core.Abstractions.Repositories;
using InvalidPasswordException = working_good.business.core.Exceptions.InvalidPasswordException;

namespace working_good.business.application.CQRS.Users.Command.SignIn;

internal sealed class SignInCommandHandler(IUserRepository userRepository, IAuthenticator authenticator, 
    IAccessTokenStorage accessTokenStorage, IPasswordManager passwordManager) : ICommandHandler<SignInCommand>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IAuthenticator _authenticator = authenticator;
    private readonly IAccessTokenStorage _accessTokenStorage = accessTokenStorage;
    private readonly IPasswordManager _passwordManager = passwordManager;

    public async Task HandleAsync(SignInCommand command, CancellationToken token)
    {
        var user = await _userRepository.GetByEmailAsync(command.Email);
        if (user is null)
        {
            throw new UserNotFoundException(command.Email);
        }
        if (!user.CanBeLogged())
        {
            throw new UserCanNotBeLoggedException(user.Id);
        }

        if (!_passwordManager.IsValidPassword(command.Password, user.Password))
        {
            throw new IncorrectPasswordException(user.Id);
        }
        var accessToken = _authenticator.CreateAccessToken(user.Id, new List<string>() { user.Role });
        _accessTokenStorage.Set(accessToken);
    }
}