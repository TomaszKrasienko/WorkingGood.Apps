using working_good.business.core.Exceptions;

namespace working_good.business.core.ValueObjects.Ticket;

public sealed class InvalidTitleException() 
    : CustomException("Title can not be empty", "invalid_title");