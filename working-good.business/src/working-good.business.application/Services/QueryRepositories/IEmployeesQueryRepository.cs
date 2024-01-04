using working_good.business.application.DTOs;

namespace working_good.business.application.Services.QueryRepositories;

public interface IEmployeesQueryRepository
{
    Task<QueryPaginationDto<IEnumerable<EmployeeDto>>> GetEmployees(int pageNumber, int pageSize,
        Guid? companyId);
}