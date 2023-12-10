using working_good.business.core.ValueObjects;

namespace working_good.business.core.Models;

public abstract class AggregateRoot(EntityId id)
{
    public EntityId Id { get; } = id;
}