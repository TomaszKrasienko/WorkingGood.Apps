using working_good.business.core.Abstractions;
using working_good.business.core.Events;
using working_good.business.core.Exceptions;
using working_good.business.core.Policies.Abstractions;
using working_good.business.core.ValueObjects.User;

namespace working_good.business.core.Models;

public class User : AggregateRoot
{
    public Email Email { get; private set; }
    public Password Password { get; private set; }
    public FullName FullName { get; private set; }
    public VerificationToken VerificationToken { get; private set; }
    public ResetPasswordToken ResetPasswordToken { get; private set; }
    public Role Role { get; private set; }

    private ISet<IDomainEvent> _events = new HashSet<IDomainEvent>();
    public IEnumerable<IDomainEvent> Events => _events;
    
    private User(Guid id, Email email, FullName fullName, Password password, Role role) : base(id)
    {
        Email = email;
        FullName = fullName;
        Password = password;
        VerificationToken = VerificationToken.Create();
        Role = role;
        _events.Add(new UserAdded(id, email, VerificationToken.Token));
    }
    
    public static User CreateUser(IPasswordPolicy userPasswordPolicy, IPasswordManager passwordManager, Guid id,
        Email email, FullName fullName, Password password, Role role)
    {
        if (!(userPasswordPolicy.VerifyPassword(password)))
            throw new ToWeakPasswordException();
        string securedPassword = passwordManager.Secure(password);
        return new User(id, email, fullName, securedPassword, role);
    }

    public void VerifyAccount(string token)
        => VerificationToken.Verify(token);

    public bool CanBeLogged()
        => VerificationToken.VerificationDate is not null;

}