using working_good.business.application.CQRS.Abstractions;
using working_good.business.application.DTOs;
using working_good.business.application.Mappers;
using working_good.business.core.Abstractions.Repositories;

namespace working_good.business.application.CQRS.Companies.Queries.GetCompanyById;

internal sealed class GetCompanyByIdQueryHandler : IQueryHandler<GetCompanyByIdQuery, CompanyDto>
{
    private readonly ICompanyRepository _companyRepository;
    
    public GetCompanyByIdQueryHandler(ICompanyRepository companyRepository)
    {
        _companyRepository = companyRepository;
    }
    public async Task<CompanyDto> HandleAsync(GetCompanyByIdQuery query, CancellationToken cancellationToken)
        => (await _companyRepository.GetByIdAsync(query.Id)).AsDto();
    
}