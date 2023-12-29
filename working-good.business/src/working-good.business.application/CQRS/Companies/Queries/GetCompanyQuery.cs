using working_good.business.application.CQRS.Abstractions;
using working_good.business.application.DTOs;

namespace working_good.business.application.CQRS.Companies.Queries;

public record GetCompanyQuery(Guid Id) : IQuery<CompanyDto>;