using working_good.business.core.Exceptions;

namespace working_good.business.core.ValueObjects.User;

public record Role
{
    public static IEnumerable<string> AvailableRolesForOwner = new[] { "Manager", "Employee" };
    public static IEnumerable<string> AvailableRolesForClient = new[] { "User" };
    public string Value { get; }

    public Role(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidUserRoleException(value);
        if (!AvailableRolesForOwner.Contains(value) && !AvailableRolesForClient.Contains(value))
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