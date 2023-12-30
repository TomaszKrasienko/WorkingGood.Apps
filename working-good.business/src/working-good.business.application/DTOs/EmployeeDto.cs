namespace working_good.business.application.DTOs;

public record EmployeeDto
{
    public Guid Id { get; set; }
    public string Email { get; init; }
    public Guid? UserId { get; init; }
}