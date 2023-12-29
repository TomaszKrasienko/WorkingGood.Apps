using working_good.business.core.Exceptions;

namespace working_good.business.core.DomainServices;

public class OwnerCompanyDoesNotExistsException()
    : CustomException("Owner company does not exists in system", "owner_company_does_not_exists");