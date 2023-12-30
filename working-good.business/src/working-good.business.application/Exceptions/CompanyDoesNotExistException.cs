using working_good.business.core.Exceptions;

namespace working_good.business.application.CQRS.Employees.Commands;

public sealed class CompanyDoesNotExistException(Guid companyId) 
    : CustomException($"Company with Id: {companyId}", "company_does_not_exist");