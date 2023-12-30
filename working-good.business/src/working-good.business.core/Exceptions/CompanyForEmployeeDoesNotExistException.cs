namespace working_good.business.core.Exceptions;

public sealed class CompanyForEmployeeDoesNotExistException(Guid employeeId)
    : CustomException($"Company with employee with Id: {employeeId} does not exist",
        "company_for_employee_does_not_exists");