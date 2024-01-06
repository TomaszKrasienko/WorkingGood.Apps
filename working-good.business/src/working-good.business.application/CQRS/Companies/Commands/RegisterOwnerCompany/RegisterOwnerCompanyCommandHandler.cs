using working_good.business.application.CQRS.Abstractions;
using working_good.business.core.Abstractions.Repositories;
using working_good.business.core.DomainServices.Abstractions;
using working_good.business.core.Models.Company;

namespace working_good.business.application.CQRS.Companies.Commands.RegisterOwnerCompany;

internal sealed class RegisterOwnerCompanyCommandHandler : ICommandHandler<RegisterOwnerCompanyCommand>
{
    private readonly ICompanyRegistrationService _companyRegistrationService;
    private readonly ICompanyRepository _companyRepository;
    private readonly IUserRegistrationService _userRegistrationService;

    public RegisterOwnerCompanyCommandHandler(ICompanyRegistrationService companyRegistrationService, 
        ICompanyRepository companyRepository, IUserRegistrationService userRegistrationService)
    {
        _companyRegistrationService = companyRegistrationService;
        _companyRepository = companyRepository;
        _userRegistrationService = userRegistrationService;
    }

    public async Task HandleAsync(RegisterOwnerCompanyCommand command, CancellationToken token)
    {
        var companies = await _companyRepository.GetAllAsync();
        var company = _companyRegistrationService.RegisterCompany(companies, command.CompanyId, command.Name,
            true, command.EmailDomain, null);
        company.AddEmployee(command.EmployeeId, command.EmployeeEmail);
        _userRegistrationService.RegisterNewUser(new List<Company> { company }, command.EmployeeId,
            command.UserId, command.FirstName, command.LastName, command.Password);
        await _companyRepository.AddAsync(company);
    }
}