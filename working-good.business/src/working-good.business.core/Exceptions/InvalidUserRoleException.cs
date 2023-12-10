namespace working_good.business.core.Exceptions;

public sealed class InvalidUserRoleException(string value) 
    : CustomException($"The role {value} is invalid", "invalid_employee_role");