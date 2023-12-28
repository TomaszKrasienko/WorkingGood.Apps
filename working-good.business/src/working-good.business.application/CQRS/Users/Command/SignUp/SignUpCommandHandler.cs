using working_good.business.application.CQRS.Abstractions;
using working_good.business.core.Abstractions;
using working_good.business.core.Abstractions.Repositories;
using working_good.business.core.DomainServices.Abstractions;
using working_good.business.core.Exceptions;
using working_good.business.core.Models;
using working_good.business.core.Policies.Abstractions;
using working_good.business.core.ValueObjects.User;

namespace working_good.business.application.CQRS.Users.Command.SignUp;

internal sealed class SignUpCommandHandler(ICompanyRepository companyRepository,
    IUserRegistrationService userRegistrationService) : ICommandHandler<SignUpCommand>
{
    public async Task HandleAsync(SignUpCommand command, CancellationToken token)
    {
        var companies = await companyRepository.GetAllAsync();
        if (companies is null || !companies.Any())
        {
            throw new CompaniesDoesNotExistException();
        }
        var company = userRegistrationService.RegisterNewUser(companies, command.CompanyId,
            command.Id, command.Email, command.FirstName, command.LastName, command.Password,
            command.Role);
        await companyRepository.UpdateAsync(company);
    }
    
}

public sealed class CompaniesDoesNotExistException()
    : CustomException("There is no registered company", "companies_does_not_exists");