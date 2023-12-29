using working_good.business.application.CQRS.Abstractions;
using working_good.business.application.DTOs;
using working_good.business.application.Mappers;
using working_good.business.core.Abstractions.Repositories;

namespace working_good.business.application.CQRS.Companies.Queries;

internal sealed class GetCompanyQueryHandler : IQueryHandler<GetCompanyQuery, CompanyDto>
{
    private readonly ICompanyRepository _companyRepository;
    
    public GetCompanyQueryHandler(ICompanyRepository companyRepository)
    {
        _companyRepository = companyRepository;
    }
    public async Task<CompanyDto> HandleAsync(GetCompanyQuery query, CancellationToken cancellationToken)
        => (await _companyRepository.GetByIdAsync(query.Id)).AsDto();
    
}