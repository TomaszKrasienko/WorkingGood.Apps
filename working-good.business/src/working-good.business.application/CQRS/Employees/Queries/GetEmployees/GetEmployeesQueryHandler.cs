using working_good.business.application.CQRS.Abstractions;
using working_good.business.application.DTOs;
using working_good.business.application.Mappers;
using working_good.business.core.Abstractions.Repositories;

namespace working_good.business.application.CQRS.Employees.Queries.GetEmployees;

internal sealed class GetEmployeesQueryHandler : IQueryHandler<GetEmployeesQuery, IEnumerable<EmployeeDto>>
{
    private readonly ICompanyRepository _companyRepository;

    public GetEmployeesQueryHandler(ICompanyRepository companyRepository)
    {
        _companyRepository = companyRepository;
    }

    public async Task<IEnumerable<EmployeeDto>> HandleAsync(GetEmployeesQuery query, CancellationToken cancellationToken)
    {
        if (query.companyId is null)
        {
            var company = await _companyRepository.GetByIdAsync((Guid)query.companyId);
            return company.Employees.Select(x => x.AsDto()).ToList();
        }

        var employees = (await _companyRepository.GetAllAsync()).SelectMany(x => x.Employees);
        return employees.Select(x => x.AsDto());
    }
}