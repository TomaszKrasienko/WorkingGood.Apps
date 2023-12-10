namespace working_good.business.core.Events;

public record UserAdded : IDomainEvent
{
    private readonly string _email;
    private readonly string _verificationToken;
    public string Code => "user_added";
    public List<string> Emails => new List<string>(){ _email };
    public Dictionary<string, string> Arguments =>
        new Dictionary<string, string>()
        {
            ["verification_token"] = _verificationToken
        };
    public UserAdded(Guid userId, string email, string verificationToken)
    {
        _email = email;
        _verificationToken = verificationToken;
    }
}