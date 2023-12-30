namespace working_good.business.application.DTOs;

public record CompanyDto
{
    public Guid Id { get; set; }
    public string Name { get; init; }
    public bool IsOwner { get; init; }
    public TimeSpan? SlaTimeSpan { get; init; }
    public string EmailDomain { get; init; }
}