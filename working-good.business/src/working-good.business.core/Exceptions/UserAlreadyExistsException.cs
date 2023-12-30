namespace working_good.business.core.Exceptions;

public class UserAlreadyExistsException(Guid employeeId) 
    : CustomException($"For employee with Id: {employeeId} user already exists", "user_already_exists");