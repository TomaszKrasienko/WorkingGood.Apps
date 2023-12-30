using working_good.business.application.CQRS.Abstractions;
using working_good.business.application.DTOs;

namespace working_good.business.application.CQRS.Employees.Queries.GetEmployeeById;

public class GetEmployeeByIdQuery(Guid employeeId) : IQuery<EmployeeDto>;
