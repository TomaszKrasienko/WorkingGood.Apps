using working_good.business.application.DTOs;
using working_good.business.core.Models.Company;

namespace working_good.business.application.Mappers;

public static class AsDtoMapper
{
    internal static CompanyDto AsDto(this Company company)
        => new CompanyDto()
        {
            Id = company.Id,
            Name = company.Name,
            EmailDomain = company.EmailDomain,
            IsOwner = company.IsOwner,
            SlaTimeSpan = company.SlaTimeSpan ?? null
        };

    internal static EmployeeDto AsDto(this Employee employee)
    {
        if (employee.User is null)
        {            
            return new EmployeeDto()
            {
                Id = employee.Id,
                Email = employee.Email
            };
        }
        else
        {
            return new EmployeeDto()
            {
                Id = employee.Id,
                Email = employee.Email,
                UserId = employee.User?.Id
            };
        }
    }
}