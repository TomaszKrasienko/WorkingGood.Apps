using working_good.business.application.DTOs;
using working_good.business.core.Models.Company;

namespace working_good.business.application.Mappers;

public static class AsDtoMapper
{
    internal static CompanyDto AsDto(this Company company)
        => new CompanyDto()
        {
            Name = company.Name,
            EmailDomain = company.EmailDomain,
            IsOwner = company.IsOwner,
            SlaTimeSpan = company.SlaTimeSpan ?? null
        };
}