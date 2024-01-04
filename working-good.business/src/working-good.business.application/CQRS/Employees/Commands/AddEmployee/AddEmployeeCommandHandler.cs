using working_good.business.application.CQRS.Abstractions;
using working_good.business.core.Abstractions.Repositories;
using working_good.business.core.DomainServices;

namespace working_good.business.application.CQRS.Employees.Commands;

internal sealed class AddEmployeeCommandHandler : ICommandHandler<AddEmployeeCommand>
{
    private readonly ICompanyRepository _companyRepository;

    public AddEmployeeCommandHandler(ICompanyRepository companyRepository)
        => _companyRepository = companyRepository;

    public async Task HandleAsync(AddEmployeeCommand command, CancellationToken token)
    {
        var company = await _companyRepository.GetByIdAsync(command.CompanyId);
        if (company is null)
        {
            throw new CompanyDoesNotExistException(command.CompanyId);
        }
        company.AddEmployee(command.Id, command.Email);
        await _companyRepository.UpdateAsync(company);
    }
}