using working_good.business.core.ValueObjects;
using working_good.business.core.ValueObjects.User;

namespace working_good.business.core.Models.Company;

public class Employee : Entity
{
    public Email Email { get; private set; }
    public User User { get; set; }

    //For EntityFramework
    private Employee() : base(Guid.NewGuid())
    {
        
    }
    
    internal Employee(EntityId entityId, Email email) : base(entityId)
    {
        Email = email;
    }
    
    
}