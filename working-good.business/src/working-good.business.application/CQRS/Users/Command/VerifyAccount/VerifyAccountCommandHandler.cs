using working_good.business.application.CQRS.Abstractions;
using working_good.business.application.Exceptions;
using working_good.business.core.Abstractions.Repositories;

namespace working_good.business.application.CQRS.Users.Command.VerifyAccount;

internal sealed class VerifyAccountCommandHandler(ICompanyRepository companyRepository)
    : ICommandHandler<VerifyAccountCommand>
{
    public async Task HandleAsync(VerifyAccountCommand command, CancellationToken token)
    {
        var company = await companyRepository.GetByUserVerificationTokenAsync(command.VerificationToken);
        if (company is null)
        {
            throw new VerificationTokenDoesNotExistsException(command.VerificationToken);
        }
        company.VerifyUser(command.VerificationToken);
        await companyRepository.UpdateAsync(company);
    }
}