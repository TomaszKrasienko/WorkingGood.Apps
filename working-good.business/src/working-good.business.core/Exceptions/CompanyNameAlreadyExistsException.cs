using working_good.business.core.Exceptions;

namespace working_good.business.core.DomainServices;

public class CompanyNameAlreadyExistsException(string name) 
    : CustomException($"Company with name {name} already exists", "company_name_already_exists");