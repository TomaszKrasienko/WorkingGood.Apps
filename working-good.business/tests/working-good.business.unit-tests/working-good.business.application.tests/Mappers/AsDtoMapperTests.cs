using FluentAssertions;
using Moq;
using working_good.business.application.Mappers;
using working_good.business.core.Abstractions;
using working_good.business.core.Models.Company;
using working_good.business.core.Policies;
using working_good.business.core.Policies.Abstractions;
using working_good.business.core.ValueObjects.User;
using Xunit;

namespace working_good.business.application.tests.Mappers;

public class AsDtoMapperTests
{
    [Fact]
    public void AsDto_ForCompanyOwner_ShouldReturnCompanyDto()
    {
        //arrange 
        var company = Company.CreateOwnerCompany(Guid.NewGuid(), "testName", "test.pl");
        
        //act
        var companyDto = company.AsDto();
        
        //assert
        companyDto.Id.Should().Be(company.Id);
        companyDto.Name.Should().Be(company.Name);
        companyDto.EmailDomain.Should().Be(company.EmailDomain);
        companyDto.IsOwner.Should().Be(company.IsOwner);
    }
    
    
    [Fact]
    public void AsDto_ForCompany_ShouldReturnCompanyDto()
    {
        //arrange 
        var company = Company.CreateCompany(Guid.NewGuid(), "testName", 
            new TimeSpan(10, 00, 00),"test.pl");
        
        //act
        var companyDto = company.AsDto();
        
        //assert
        companyDto.Id.Should().Be(company.Id);
        companyDto.Name.Should().Be(company.Name);
        companyDto.EmailDomain.Should().Be(company.EmailDomain);
        companyDto.IsOwner.Should().Be(company.IsOwner);
        companyDto.SlaTimeSpan.Should().Be(company.SlaTimeSpan);
    }
    
    [Fact]
    public void AsDto_ForEmployeeWithoutUser_ShouldReturnEmployeeDto()
    {
        //arrange 
        Employee employee = new Employee(Guid.NewGuid(), "test@test.pl");
        
        //act
        var employeeDto = employee.AsDto();
        
        //assert
        employeeDto.Id.Should().Be(employee.Id);
        employeeDto.Email.Should().Be(employee.Email);
        employeeDto.UserId.Should().BeNull();
    }
    
    [Fact]
    public void AsDto_ForEmployeeWithUser_ShouldReturnEmployeeDto()
    {
        //arrange 
        Employee employee = new Employee(Guid.NewGuid(), "test@test.pl");
        _passwordManagerMock
            .Setup(f => f.Secure(It.IsAny<string>()))
            .Returns("securedPassword");
        employee.User = User.CreateUser(_passwordPolicy, _passwordManagerMock.Object, Guid.NewGuid(),
            new FullName("testFirstName", "testLastName"), "Test123#", Role.User(), employee.Id);
        
        //act
        var employeeDto = employee.AsDto();
        
        //assert
        employeeDto.Id.Should().Be(employee.Id);
        employeeDto.Email.Should().Be(employee.Email);
        employeeDto.UserId.Should().Be(employee.User.Id);
    }

    #region arrange

    private readonly IPasswordPolicy _passwordPolicy;
    private readonly Mock<IPasswordManager> _passwordManagerMock;

    public AsDtoMapperTests()
    {
        _passwordPolicy = new UserPasswordPolicy();
        _passwordManagerMock = new Mock<IPasswordManager>();
    }
    #endregion
}