using working_good.business.core.Exceptions;

namespace working_good.business.core.DomainServices;

public sealed class CompanyDoesNotExistException(Guid companyId)
    : CustomException($"Company with Id: {companyId} does not exist");