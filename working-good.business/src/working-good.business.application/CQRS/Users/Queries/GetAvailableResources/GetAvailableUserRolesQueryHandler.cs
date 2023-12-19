using working_good.business.application.CQRS.Abstractions;
using working_good.business.core.ValueObjects.User;

namespace working_good.business.application.CQRS.Users.Queries.GetAvailableResources;

internal sealed class GetAvailableUserRolesQueryHandler : IQueryHandler<GetAvailableUserRolesQuery, List<string>>
{
    public Task<List<string>> HandleAsync(GetAvailableUserRolesQuery query, CancellationToken cancellationToken)
        => Task.FromResult(Role.AvailableRoles.ToList());
}