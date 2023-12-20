using working_good.business.core.Exceptions;

namespace working_good.business.application.Exceptions;

public sealed class VerificationTokenDoesNotExistsException(string verificationToken) : AuthorizeCustomException(
    $"User for verification token: {verificationToken} does not exists", "verification_token_does_not_exists");