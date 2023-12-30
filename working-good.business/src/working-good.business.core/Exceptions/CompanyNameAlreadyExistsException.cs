namespace working_good.business.core.Exceptions;

public class CompanyNameAlreadyExistsException(string name) 
    : CustomException($"Company with name {name} already exists", "company_name_already_exists");