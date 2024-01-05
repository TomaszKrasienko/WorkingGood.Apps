using FluentAssertions;
using Moq;
using working_good.business.application.CQRS.Employees.Commands;
using working_good.business.core.Abstractions.Repositories;
using working_good.business.core.DomainServices;
using working_good.business.core.Exceptions;
using working_good.business.core.Models.Company;
using Xunit;

namespace working_good.business.application.tests.CQRS.Employees.Commands;

public sealed class AddEmployeeCommandHandlerTests
{
    [Fact]
    public async Task
        HandleAsync_ForAddEmployeeCommandWithExistingCompanyId_ShouldAddEmployeeToCompanyAndUpdateCompany()
    {
        //arrange
        Company company = Company.CreateCompany(Guid.NewGuid(), "Name", new TimeSpan(1000),
            "test.pl");
        _companyRepositoryMock
            .Setup(f => f.GetByIdAsync(It.Is<Guid>(arg
                => arg == company.Id)))
            .ReturnsAsync(company);
        var command = new AddEmployeeCommand(company.Id, Guid.NewGuid(), "test@test.pl");
        
        //act
        await _handler.HandleAsync(command, default);
        
        //assert
        company.Employees.Any().Should().BeTrue();
        _companyRepositoryMock
            .Verify(f => f.UpdateAsync(It.Is<Company>(arg 
                => arg == company)));
    }
    
    [Fact]
    public async Task HandleAsync_ForNotExistingCompanyId_ShouldThrowCompanyDoesNotExistException()
    {
        //arrange
        var command = new AddEmployeeCommand(Guid.NewGuid(), Guid.NewGuid(), "test@test.pl");
        
        //act
        var exception = await Record.ExceptionAsync(async() => await _handler.HandleAsync(command, default));
        
        //assert
        exception.Should().BeOfType<CompanyDoesNotExistException>();
    }
    
    private readonly Mock<ICompanyRepository> _companyRepositoryMock;
    private readonly AddEmployeeCommandHandler _handler;
    public AddEmployeeCommandHandlerTests()
    {
        _companyRepositoryMock = new Mock<ICompanyRepository>();
        _handler = new AddEmployeeCommandHandler(_companyRepositoryMock.Object);
    }
}