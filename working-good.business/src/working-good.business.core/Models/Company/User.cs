using working_good.business.core.Abstractions;
using working_good.business.core.Exceptions;
using working_good.business.core.Policies.Abstractions;
using working_good.business.core.ValueObjects;
using working_good.business.core.ValueObjects.User;

namespace working_good.business.core.Models.Company;

public sealed class User : Entity
{
    public Password Password { get; private set; }
    public FullName FullName { get; private set; }
    public VerificationToken VerificationToken { get; private set; }
    public ResetPasswordToken ResetPasswordToken { get; private set; }
    public Role Role { get; private set; }
    public EntityId EmployeeId { get; private set; }

    //For EntityFramework
    private User() : base(Guid.NewGuid())
    {
        
    }
    
    private User(Guid id, FullName fullName, Password password, Role role, EntityId employeeId) : base(id)
    {
        FullName = fullName;
        Password = password;
        VerificationToken = VerificationToken.Create();
        Role = role;
        EmployeeId = employeeId;
    }
    
    internal static User CreateUser(IPasswordPolicy userPasswordPolicy, IPasswordManager passwordManager, Guid id,
        FullName fullName, Password password, Role role, EntityId employeeId)
    {
        if (!(userPasswordPolicy.VerifyPassword(password)))
            throw new ToWeakPasswordException();
        string securedPassword = passwordManager.Secure(password);
        return new User(id, fullName, securedPassword, role, employeeId);
    }

    internal void VerifyAccount(string token)
        => VerificationToken.Verify(token);

    internal bool CanBeLogged()
        => VerificationToken.VerificationDate is not null;

}