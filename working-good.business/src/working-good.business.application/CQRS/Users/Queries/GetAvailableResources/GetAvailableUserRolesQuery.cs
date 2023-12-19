using working_good.business.application.CQRS.Abstractions;

namespace working_good.business.application.CQRS.Users.Queries.GetAvailableResources;

public record GetAvailableUserRolesQuery : IQuery<List<string>>;