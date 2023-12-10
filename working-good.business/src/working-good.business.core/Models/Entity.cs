using working_good.business.core.ValueObjects;

namespace working_good.business.core.Models;

internal class Entity
{
    public EntityId Id { get; }

    protected Entity(EntityId id)
    {
        Id = id;
    }
}