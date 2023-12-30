using working_good.business.core.ValueObjects;

namespace working_good.business.core.Exceptions;

public class EmployeeDoesNotExistException(EntityId employeeId) 
    : CustomException($"Employee with Id: {employeeId} does not exist", "employee_does_not_exist");