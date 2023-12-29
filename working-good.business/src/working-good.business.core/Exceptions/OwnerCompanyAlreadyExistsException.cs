using working_good.business.core.Exceptions;

namespace working_good.business.core.DomainServices;

public class OwnerCompanyAlreadyExistsException() 
    : CustomException("Owner company already exists", "owner_company_already_exists");