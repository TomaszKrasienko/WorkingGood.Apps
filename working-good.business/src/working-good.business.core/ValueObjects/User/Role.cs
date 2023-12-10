using working_good.business.core.Exceptions;

namespace working_good.business.core.ValueObjects.User;

public record Role
{
    public static IEnumerable<string> AvailableRoles = new[] {"Manager", "Employee", "User"};

    public string Value { get; }

    public Role(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidUserRoleException(value);
        if (!AvailableRoles.Contains(value))
            throw new InvalidUserRoleException(value);
        Value = value;
    }

    public static Role Manager() => new Role("Manager");

    public static Role Employee() => new Role("Employee");

    public static Role User() => new Role("User");

    public static implicit operator string(Role role)
        => role.Value;

    public static implicit operator Role(string value)
        => new Role(value);
}