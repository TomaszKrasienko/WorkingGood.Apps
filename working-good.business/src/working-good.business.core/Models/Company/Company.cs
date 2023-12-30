using working_good.business.core.Abstractions;
using working_good.business.core.Exceptions;
using working_good.business.core.Policies.Abstractions;
using working_good.business.core.ValueObjects;
using working_good.business.core.ValueObjects.Company;
using working_good.business.core.ValueObjects.User;

namespace working_good.business.core.Models.Company;

public sealed class Company : AggregateRoot
{
    public Name Name { get; private set; }
    public IsOwner IsOwner { get; private set; }
    public SlaTimeSpan SlaTimeSpan { get; private set; }
    public EmailDomain EmailDomain { get; private set; }
    private ISet<Employee> _employees = new HashSet<Employee>();
    public IEnumerable<Employee> Employees => _employees;
    
    private Company(EntityId entityId, Name name, IsOwner isOwner, SlaTimeSpan slaTimeSpan,
        EmailDomain emailDomain) 
        : base(entityId)
    {
        Name = name;
        IsOwner = isOwner;
        SlaTimeSpan = slaTimeSpan;
        EmailDomain = emailDomain;
    }

    internal static Company CreateOwnerCompany(EntityId entityId, Name name, EmailDomain emailDomain)
        => new Company(entityId, name, true, null, emailDomain);

    internal static Company CreateCompany(EntityId entityId, Name name, SlaTimeSpan slaTimeSpan,
        EmailDomain emailDomain)
        => new Company(entityId, name, false, slaTimeSpan, emailDomain);

    public void AddEmployee(EntityId guid, Email email)
    {
        if (_employees.Any(x => x.Email == email))
        {
            throw new EmailAlreadyInUseException(email);
        }
        string employeeDomain = email.Value.Substring(email.Value.IndexOf("@", StringComparison.Ordinal) + 1);
        
        if (employeeDomain != EmailDomain.Value)
        {
            throw new NotMatchingEmployeeEmailDomainException(EmailDomain);
        }
        _employees.Add(new Employee(guid, email));
    }
    
    internal void RegisterUser(IPasswordPolicy userPasswordPolicy, IPasswordManager passwordManager, EntityId employeeId, 
        EntityId id, FullName fullName, Password password, Role role)
    {
        var employee = _employees.FirstOrDefault(x => x.Id.Equals(employeeId));
        if (employee is null)
        {
            throw new EmployeeDoesNotExistException(employeeId);
        }

        var user = User.CreateUser(userPasswordPolicy, passwordManager, id, fullName, password, role);
        employee.User = user;
    }

    public void VerifyUser(string token)
    {
        var employee = _employees.Single(x => x.User.VerificationToken.Token == token);
        employee.User.VerifyAccount(token);
    }

    public bool CanUserBeLogged(string email)
    {
        var employee = _employees.Single(x => x.Email == email);
        return employee.User is not null && employee.User.CanBeLogged();
    }
}