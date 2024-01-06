using working_good.business.application.CQRS.Abstractions;
using working_good.business.application.DTOs;

namespace working_good.business.application.CQRS.Companies.Queries.IsOwnerCompanyRegistered;

public record IsOwnerCompanyRegisteredQuery() : IQuery<bool>;