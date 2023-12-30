using working_good.business.core.Exceptions;

namespace working_good.business.application.Exceptions;

public sealed class CompaniesDoesNotExistException()
    : CustomException("There is no registered company", "companies_does_not_exists");