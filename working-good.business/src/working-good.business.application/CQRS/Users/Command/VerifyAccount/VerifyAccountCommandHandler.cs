using working_good.business.application.CQRS.Abstractions;
using working_good.business.application.Exceptions;
using working_good.business.core.Abstractions.Repositories;

namespace working_good.business.application.CQRS.Users.Command.VerifyAccount;

internal sealed class VerifyAccountCommandHandler(IUserRepository userRepository)
    : ICommandHandler<VerifyAccountCommand>
{
    private readonly IUserRepository _userRepository = userRepository;

    public async Task HandleAsync(VerifyAccountCommand command, CancellationToken token)
    {
        var user = await _userRepository.GetByVerificationToken(command.VerificationToken);
        if (user is null)
        {
            throw new VerificationTokenDoesNotExistsException(command.VerificationToken);
        }
        
        user.VerifyAccount(command.VerificationToken);
        await _userRepository.UpdateAsync(user);
    }
}