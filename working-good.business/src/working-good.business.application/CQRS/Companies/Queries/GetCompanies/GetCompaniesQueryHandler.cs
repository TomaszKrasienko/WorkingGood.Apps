using working_good.business.application.CQRS.Abstractions;
using working_good.business.application.DTOs;
using working_good.business.application.Mappers;
using working_good.business.core.Abstractions.Repositories;

namespace working_good.business.application.CQRS.Companies.Queries.GetCompanies;

internal sealed class GetCompaniesQueryHandler : IQueryHandler<GetCompaniesQuery, IEnumerable<CompanyDto>>
{
    private readonly ICompanyRepository _companyRepository;

    public GetCompaniesQueryHandler(ICompanyRepository companyRepository)
    {
        _companyRepository = companyRepository;
    }
    
    public async Task<IEnumerable<CompanyDto>> HandleAsync(GetCompaniesQuery query, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrEmpty(query.Name))
        {
            return (await _companyRepository.GetAllAsync())
                .Where(x 
                    => x.Name.Value.Contains(query.Name) 
                       && x.IsOwner.Value == !query.IsClient)
                .Select(x => x.AsDto());
        }
        else
        {
             return (await _companyRepository.GetAllAsync())
                .Where(x 
                    =>  x.IsOwner.Value == !query.IsClient)
                .Select(x => x.AsDto());
        }

    }
}