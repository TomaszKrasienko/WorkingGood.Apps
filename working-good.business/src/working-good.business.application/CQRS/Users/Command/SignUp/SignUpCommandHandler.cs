using working_good.business.application.CQRS.Abstractions;
using working_good.business.application.Exceptions;
using working_good.business.core.Abstractions.Repositories;
using working_good.business.core.DomainServices.Abstractions;

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
        var company = userRegistrationService.RegisterNewUser(companies, command.EmployeeId,command.Id,  
            command.FirstName, command.LastName, command.Password);
        await companyRepository.UpdateAsync(company);
    }
    
}