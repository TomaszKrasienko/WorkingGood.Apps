using working_good.business.application.CQRS.Abstractions;
using working_good.business.application.DTOs;

namespace working_good.business.application.CQRS.Companies.Queries.GetCompanies;

internal sealed class GetCompaniesQueryHandler : IQueryHandler<GetCompaniesQuery, IEnumerable<CompanyDto>>
{
    public GetCompaniesQueryHandler()
    {
        
    }
    
    public Task<IEnumerable<CompanyDto>> HandleAsync(GetCompaniesQuery query, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}