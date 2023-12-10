namespace working_good.business.core.Exceptions;

public sealed class InvalidEntityIdException() 
    : CustomException("Entity id can not be empty", "invalid_entity_id");