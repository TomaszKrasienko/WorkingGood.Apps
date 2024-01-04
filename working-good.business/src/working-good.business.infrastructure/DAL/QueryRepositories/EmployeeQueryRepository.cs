using System.Data;
using Dapper;
using working_good.business.application.DTOs;
using working_good.business.application.Services.QueryRepositories;

namespace working_good.business.infrastructure.DAL.QueryRepositories;

internal sealed class EmployeeQueryRepository(IWgDbConnection dbConnection) : IEmployeesQueryRepository
{
    public async Task<QueryPaginationDto<IEnumerable<EmployeeDto>>> GetEmployees(int pageNumber, int pageSize, Guid? companyId)
    {
        var procedure = "wg.GetEmployeesList";
        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("@pageNumber", pageNumber);
        parameters.Add("@pageSize", pageSize);
        if (companyId is not null)
        {
            parameters.Add("@companyId", companyId);
        }
        
        using var connection = dbConnection.DbConnection;
        var results = await connection.QueryMultipleAsync(sql: procedure,
            param: parameters, commandType: CommandType.StoredProcedure);
        return new QueryPaginationDto<IEnumerable<EmployeeDto>>(
            (await results.ReadAsync<EmployeeDto>().ConfigureAwait(false)).ToList(),
            (await results.ReadAsync<PaginationDto>().ConfigureAwait(false)).FirstOrDefault()
        );
    }
}