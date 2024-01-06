using System.Data;
using Dapper;
using working_good.business.application.DTOs;
using working_good.business.application.Services.QueryRepositories;
using working_good.business.core.ValueObjects.Company;

namespace working_good.business.infrastructure.DAL.QueryRepositories;

internal sealed class CompanyQueryRepository(IWgDbConnection dbConnection) : ICompanyQueryRepository
{
    public async Task<QueryPaginationDto<IEnumerable<CompanyDto>>> GetCompaniesList(int pageNumber, int pageSize,
        string name = null, bool? isOwner = null)
    {
        var procedure = "wg.GetCompaniesList";
        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("@pageNumber", pageNumber);
        parameters.Add("@pageSize", pageSize);
        if (name is not null)
        {
            parameters.Add("@companyName", name);
        }
        if (isOwner is not null)
        {
            parameters.Add("@isOwner", isOwner);
        }

        using var connection = dbConnection.DbConnection;
        var results = await connection.QueryMultipleAsync(sql: procedure,
            param: parameters, commandType: CommandType.StoredProcedure);
        return new QueryPaginationDto<IEnumerable<CompanyDto>>(
            (await results.ReadAsync<CompanyDto>().ConfigureAwait(false)).ToList(),
            (await results.ReadAsync<PaginationDto>().ConfigureAwait(false)).FirstOrDefault()
        );
    }

    public async Task<CompanyDto> GetCompanyById(Guid id)
    {
        string query = @$"
           SELECT 
                  [Id] AS {nameof(CompanyDto.Id)}
                , [Name] AS {nameof(CompanyDto.Name)}
                , [IsOwner] AS {nameof(CompanyDto.IsOwner)}
                , [SlaTimeSpan] AS {nameof(CompanyDto.SlaTimeSpan)}
                , [EmailDomain] AS {nameof(CompanyDto.EmailDomain)}
            FROM wg.Companies comp
            WHERE 1=1
              AND comp.Id = @companyId";
        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("@companyId", id);
        using var connection = dbConnection.DbConnection;
        var result = await connection.QueryFirstOrDefaultAsync<CompanyDto>(sql: query,
            param: parameters);
        return result;
    }

    public async Task<bool> IsOwnerCompanyRegistered()
    {
        string query = @"
            SELECT 
                COUNT(1)
            FROM wg.Companies";
        using var connection = dbConnection.DbConnection;
        var result = await connection.QueryFirstAsync<int>(sql: query);
        return result > 0;
    }
}