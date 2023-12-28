using working_good.business.core.Abstractions;
using working_good.business.core.Policies.Abstractions;
using working_good.business.core.ValueObjects;
using working_good.business.core.ValueObjects.User;

namespace working_good.business.core.Models.Company;

public sealed class Company : AggregateRoot
{
    public Name Name { get; private set; }
    public IsOwner IsOwner { get; private set; }
    public SlaTimeSpan SlaTimeSpan { get; private set; }
    public EmailDomain EmailDomain { get; private set; }
    private ISet<User> _users = new HashSet<User>();
    public IEnumerable<User> Users => _users;
    
    private Company(EntityId entityId, Name name, IsOwner isOwner, SlaTimeSpan slaTimeSpan,
        EmailDomain emailDomain) 
        : base(entityId)
    {
        Name = name;
        IsOwner = isOwner;
        SlaTimeSpan = slaTimeSpan;
        EmailDomain = emailDomain;
    }

    public static Company CreateOwnerCompany(EntityId entityId, Name name, EmailDomain emailDomain)
        => new Company(entityId, name, true, null, emailDomain);

    public static Company CreateCompany(EntityId entityId, Name name, SlaTimeSpan slaTimeSpan,
        EmailDomain emailDomain)
        => new Company(entityId, name, false, slaTimeSpan, emailDomain);

    internal User RegisterUser(IPasswordPolicy userPasswordPolicy, IPasswordManager passwordManager, Guid id,
        Email email, FullName fullName, Password password, Role role)
    {
        string userDomain = email.Value.Substring(email.Value.IndexOf("@", StringComparison.Ordinal) + 1);
        
        if (userDomain != EmailDomain.Value)
        {
            throw new InvalidUserEmailDomainException(EmailDomain);
        }
        var user = User.CreateUser(userPasswordPolicy, passwordManager, id, email, fullName, password, role);
        _users.Add(user);
        return user;
    }

    public void VerifyUser(string token)
    {
        var user = _users.Single(x => x.VerificationToken.Token == token);
        user.VerifyAccount(token);
    }

    public bool CanUserBeLogged(string userEmail)
    {
        var user = _users.Single(x => x.Email == userEmail);
        return user.CanBeLogged();
    }
    
    
}