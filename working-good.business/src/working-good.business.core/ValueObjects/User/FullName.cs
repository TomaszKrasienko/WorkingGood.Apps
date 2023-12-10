using working_good.business.core.Exceptions;

namespace working_good.business.core.ValueObjects.User;

public sealed record FullName
{
    public string FirstName { get; }
    public string LastName { get; }
    
    public FullName(string firstName, string lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
            throw new InvalidFullNameException(firstName, lastName);
        FirstName = firstName;
        LastName = lastName;
    }
}