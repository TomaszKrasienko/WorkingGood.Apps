using working_good.business.application.CQRS.Abstractions;
using working_good.business.core.Abstractions.Repositories;
using working_good.business.core.DomainServices.Abstractions;

namespace working_good.business.application.CQRS.Companies.Commands.Register;

internal sealed class RegisterCompanyCommandHandler : ICommandHandler<RegisterCompanyCommand>
{
    private readonly ICompanyRegistrationService _companyRegistrationService;
    private readonly ICompanyRepository _companyRepository;

    public RegisterCompanyCommandHandler(ICompanyRegistrationService companyRegistrationService, ICompanyRepository companyRepository)
    {
        _companyRegistrationService = companyRegistrationService;
        _companyRepository = companyRepository;
    }

    public async Task HandleAsync(RegisterCompanyCommand command, CancellationToken token)
    {
        var companies = await _companyRepository.GetAllAsync();
        var company = _companyRegistrationService.RegisterCompany(companies, command.Id, command.Name,
            false, command.EmailDomain, command.SlaTimeSpan);
        await _companyRepository.AddAsync(company);
    }
}