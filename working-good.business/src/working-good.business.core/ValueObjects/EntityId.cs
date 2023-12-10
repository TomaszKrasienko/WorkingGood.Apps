using working_good.business.core.Exceptions;
using working_good.business.core.Models;

namespace working_good.business.core.ValueObjects;

public class EntityId : IEquatable<EntityId>
{
    public Guid Value { get; }

    public EntityId(Guid value)
    {
        if (value == Guid.Empty)
            throw new InvalidEntityIdException();
        Value = value;
    }
    
    public static implicit operator Guid(EntityId id)
        => id.Value;

    public static implicit operator EntityId(Guid id)
        => new EntityId(id);

    public bool Equals(EntityId? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Value.Equals(other.Value);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((EntityId) obj);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }
}