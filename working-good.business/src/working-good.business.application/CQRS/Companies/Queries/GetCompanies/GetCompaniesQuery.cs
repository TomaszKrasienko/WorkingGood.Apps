using working_good.business.application.CQRS.Abstractions;
using working_good.business.application.DTOs;

namespace working_good.business.application.CQRS.Companies.Queries.GetCompanies;

public record GetCompaniesQuery(bool? IsOwner = null, string CompanyName = null)
    : PaginationArgumentsDto, IQuery<QueryPaginationDto<IEnumerable<CompanyDto>>>;