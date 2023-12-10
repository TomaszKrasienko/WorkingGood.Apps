namespace working_good.business.core.Exceptions;

public sealed class InvalidAccountVerificationException()
    : CustomException("Verification token is different from existing", "invalid_account_verification");