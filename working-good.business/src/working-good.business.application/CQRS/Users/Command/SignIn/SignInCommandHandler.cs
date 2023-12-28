using working_good.business.application.CQRS.Abstractions;
using working_good.business.application.Exceptions;
using working_good.business.application.Services;
using working_good.business.core.Abstractions;
using working_good.business.core.Abstractions.Repositories;
using InvalidPasswordException = working_good.business.core.Exceptions.InvalidPasswordException;

namespace working_good.business.application.CQRS.Users.Command.SignIn;

internal sealed class SignInCommandHandler(ICompanyRepository companyRepository, IAuthenticator authenticator, 
    IAccessTokenStorage accessTokenStorage, IPasswordManager passwordManager) : ICommandHandler<SignInCommand>
{

    public async Task HandleAsync(SignInCommand command, CancellationToken token)
    {
        var company = await companyRepository.GetByUserEmailAsync(command.Email);
        if (company is null)
        {
            throw new UserNotFoundException(command.Email);
        }
        if (!company.CanUserBeLogged(command.Email))
        {
            throw new UserCanNotBeLoggedException(command.Email);
        }

        var user = company.Users.Single(x => x.Email == command.Email);
        if (!passwordManager.IsValidPassword(command.Password, user.Password))
        {
            throw new IncorrectPasswordException(user.Id);
        }
        var accessToken = authenticator.CreateAccessToken(user.Id, new List<string>() { user.Role });
        accessTokenStorage.Set(accessToken);
    }
}
