using working_good.business.application.CQRS.Abstractions;
using working_good.business.application.DTOs;
using working_good.business.application.Mappers;
using working_good.business.application.Services.QueryRepositories;
using working_good.business.core.Abstractions.Repositories;

namespace working_good.business.application.CQRS.Companies.Queries.GetCompanyById;

internal sealed class GetCompanyByIdQueryHandler : IQueryHandler<GetCompanyByIdQuery, CompanyDto>
{
    private readonly ICompanyQueryRepository _companyQueryRepository;
    
    public GetCompanyByIdQueryHandler(ICompanyQueryRepository companyQueryRepository)
    {
        _companyQueryRepository = companyQueryRepository;
    }

    public Task<CompanyDto> HandleAsync(GetCompanyByIdQuery query, CancellationToken cancellationToken)
        => _companyQueryRepository.GetCompanyById(query.Id);

}