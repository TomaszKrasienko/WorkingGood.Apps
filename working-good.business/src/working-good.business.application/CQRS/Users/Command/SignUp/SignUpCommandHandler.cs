using working_good.business.application.CQRS.Abstractions;
using working_good.business.core.Abstractions;
using working_good.business.core.Abstractions.Repositories;
using working_good.business.core.Models;
using working_good.business.core.Policies.Abstractions;
using working_good.business.core.ValueObjects.User;

namespace working_good.business.application.CQRS.Users.Command.SignUp;

internal sealed class SignUpCommandHandler(IUserRepository userRepository, IPasswordManager passwordManager, 
    IPasswordPolicy passwordPolicy) : ICommandHandler<SignUpCommand>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IPasswordManager _passwordManager = passwordManager;
    private readonly IPasswordPolicy _passwordPolicy = passwordPolicy;

    public async Task HandleAsync(SignUpCommand command, CancellationToken token)
    {
        var existingUser = await _userRepository.GetByEmailAsync(command.Email);
        if (existingUser is not null)
        {
            throw new EmailAlreadyExistException(command.Email);
        }

        var user = User.CreateUser(_passwordPolicy, _passwordManager, command.Id, command.Email,
            new FullName(command.FirstName, command.LastName), command.Password, command.Role);
        await _userRepository.AddAsync(user);
    }
}