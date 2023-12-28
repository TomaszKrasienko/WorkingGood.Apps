using Microsoft.VisualBasic.CompilerServices;
using working_good.business.core.Exceptions;
using working_good.business.core.ValueObjects;
using working_good.business.core.ValueObjects.Ticket;

namespace working_good.business.core.Models;

public sealed class Ticket : AggregateRoot
{
    public Title Title { get; private set; }
    public Content Content { get; private set; }
    public Status Status { get; private set; }
    public EntityId AuthorId { get; private set; }
    
    public Ticket(EntityId id, Title title) : base(id)
    {
        Title = title;
    }
}

public record Content(string Value)
{
    public static implicit operator Content(string value)
        => new Content(value);

    public static implicit operator string(Content content)
        => content.Value;
}

public record Status
{
    public static IEnumerable<string> AvailableStatuses = new[] { "New", "Assigned", "Done", "Cancelled" };
    public string Value { get; private set; }
    
    public Status(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidTicketStatusException(value);
        if (!AvailableStatuses.Contains(value))
            throw new InvalidTicketStatusException(value);
        Value = value;
    }

    public static Status New() => new Status("New");
    public static Status Assigned() => new Status("Assigned");
    public static Status Done() => new Status("Done");
    public static Status Cancelled() => new Status("Cancelled");

    public static implicit operator Status(string value)
        => new Status(value);

    public static implicit operator string(Status value)
        => value.Value;
}

public sealed class InvalidTicketStatusException(string value)
    : CustomException($"Ticket status: {value} is invalid", "invalid_ticket_status");

public sealed record TicketTime(DateTime Value)
{
    public DateTime Value { get; private set; } = Value;

    public static TicketTime Now() => new TicketTime(DateTime.Now);
}