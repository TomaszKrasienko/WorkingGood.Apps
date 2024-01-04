using System.Collections;
using working_good.business.application.CQRS.Abstractions;
using working_good.business.application.DTOs;
using working_good.business.application.Mappers;
using working_good.business.application.Services.QueryRepositories;
using working_good.business.core.Abstractions.Repositories;

namespace working_good.business.application.CQRS.Employees.Queries.GetEmployees;

internal sealed class GetEmployeesQueryHandler : IQueryHandler<GetEmployeesQuery, QueryPaginationDto<IEnumerable<EmployeeDto>>>
{
    private readonly IEmployeesQueryRepository _employeesQueryRepository;

    public GetEmployeesQueryHandler(IEmployeesQueryRepository employeesQueryRepository)
    {
        _employeesQueryRepository = employeesQueryRepository;
    }

    public Task<QueryPaginationDto<IEnumerable<EmployeeDto>>> HandleAsync(GetEmployeesQuery query,
        CancellationToken cancellationToken)
        => _employeesQueryRepository.GetEmployees(query.PageNumber, query.PageSize, query.CompanyId);
}