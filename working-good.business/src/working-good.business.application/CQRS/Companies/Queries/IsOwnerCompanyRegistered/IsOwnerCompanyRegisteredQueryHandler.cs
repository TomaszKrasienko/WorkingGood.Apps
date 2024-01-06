using working_good.business.application.CQRS.Abstractions;
using working_good.business.application.Services.QueryRepositories;

namespace working_good.business.application.CQRS.Companies.Queries.IsOwnerCompanyRegistered;

internal sealed class IsOwnerCompanyRegisteredQueryHandler : IQueryHandler<IsOwnerCompanyRegisteredQuery, bool>
{
    private readonly ICompanyQueryRepository _companyQueryRepository;

    public IsOwnerCompanyRegisteredQueryHandler(ICompanyQueryRepository companyQueryRepository)
        => _companyQueryRepository = companyQueryRepository;

    public Task<bool> HandleAsync(IsOwnerCompanyRegisteredQuery query, CancellationToken cancellationToken)
        => _companyQueryRepository.IsOwnerCompanyRegistered();
}