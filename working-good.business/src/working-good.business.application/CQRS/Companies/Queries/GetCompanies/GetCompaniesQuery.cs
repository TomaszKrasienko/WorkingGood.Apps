using working_good.business.application.CQRS.Abstractions;
using working_good.business.application.DTOs;

namespace working_good.business.application.CQRS.Companies.Queries.GetCompanies;

public record GetCompaniesQuery(string Name): IQuery<IEnumerable<CompanyDto>>;