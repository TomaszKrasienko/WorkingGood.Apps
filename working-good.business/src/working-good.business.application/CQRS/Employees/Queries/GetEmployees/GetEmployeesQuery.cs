using working_good.business.application.CQRS.Abstractions;
using working_good.business.application.DTOs;

namespace working_good.business.application.CQRS.Employees.Queries.GetEmployees;

public record GetEmployeesQuery(Guid? CompanyId) : PaginationArgumentsDto, IQuery<QueryPaginationDto<IEnumerable<EmployeeDto>>>;