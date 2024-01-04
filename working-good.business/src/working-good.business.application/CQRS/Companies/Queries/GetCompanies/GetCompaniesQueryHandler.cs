using working_good.business.application.CQRS.Abstractions;
using working_good.business.application.DTOs;
using working_good.business.application.Services.QueryRepositories;

namespace working_good.business.application.CQRS.Companies.Queries.GetCompanies;

internal sealed class GetCompaniesQueryHandler : IQueryHandler<GetCompaniesQuery, QueryPaginationDto<IEnumerable<CompanyDto>>>
{
    private readonly ICompanyQueryRepository _companyQueryRepository;
    public GetCompaniesQueryHandler(ICompanyQueryRepository companyQueryRepository)
    {
        _companyQueryRepository = companyQueryRepository;
    }

    public async Task<QueryPaginationDto<IEnumerable<CompanyDto>>> HandleAsync(GetCompaniesQuery query, CancellationToken cancellationToken)
        => await _companyQueryRepository.GetCompaniesList(query.PageNumber, query.PageSize, query.CompanyName,
            query.IsOwner);
}