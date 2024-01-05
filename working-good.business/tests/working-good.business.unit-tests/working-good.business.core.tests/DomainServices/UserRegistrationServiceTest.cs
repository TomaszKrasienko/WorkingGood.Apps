using FluentAssertions;
using Moq;
using working_good.business.core.Abstractions;
using working_good.business.core.DomainServices;
using working_good.business.core.DomainServices.Abstractions;
using working_good.business.core.Exceptions;
using working_good.business.core.Models.Company;
using working_good.business.core.Policies;
using working_good.business.core.Policies.Abstractions;
using working_good.business.core.ValueObjects;
using working_good.business.core.ValueObjects.User;
using Xunit;

namespace working_good.business.core.tests.DomainServices;

public sealed class UserRegistrationServiceTest
{
    [Fact]
    public void RegisterNewUser_ForExistingEmployeeAndClientCompany_ShouldReturnCompanyWithEmployeeAndUserWithRoleUser()
    {
        //arrange
        var company = Company.CreateCompany(Guid.NewGuid(), "testCompanyName", 
            new TimeSpan(00, 00, 10), "test.pl");
        company.AddEmployee(Guid.NewGuid(), "test@test.pl");
        var employeeId = company.Employees.Single().Id;
        
        string password = "Test123!";
        Guid userId = Guid.NewGuid();
        _passwordManagerMock
            .Setup(f => f.Secure(It.Is<string>(arg => arg == password)))
            .Returns("securedPassword");
        
        //act
        var result = _userRegistrationService.RegisterNewUser([company], employeeId,
            userId, "firstName", "lastName", password);
        
        //assert
        result.Should().BeOfType<Company>();
        result.Employees.Any(x 
            => x.Id.Equals(employeeId)
            && x.User is not null
            && x.User.Id == userId).Should().BeTrue();
        result.Employees.First(x => x.Id.Equals(employeeId)).User.Role.Should().Be(Role.User());
    }
    
    [Fact]
    public void RegisterNewUser_ForExistingEmployeeFirstUserAndOwnerCompany_ShouldReturnCompanyWithEmployeeAndUserWithRoleManager()
    {
        //arrange
        var company = Company.CreateOwnerCompany(Guid.NewGuid(), "testCompanyName", "test.pl");
        company.AddEmployee(Guid.NewGuid(), "test@test.pl");
        var employeeId = company.Employees.Single().Id;
        
        string password = "Test123!";
        Guid userId = Guid.NewGuid();
        _passwordManagerMock
            .Setup(f => f.Secure(It.Is<string>(arg => arg == password)))
            .Returns("securedPassword");
        
        //act
        var result = _userRegistrationService.RegisterNewUser([company], employeeId,
            userId, "firstName", "lastName", password);
        
        //assert
        result.Should().BeOfType<Company>();
        result.Employees.Any(x 
            => x.Id.Equals(employeeId)
               && x.User is not null
               && x.User.Id == userId).Should().BeTrue();
        result.Employees.First(x => x.Id.Equals(employeeId)).User.Role.Should().Be(Role.Manager());
    }
    
    [Fact]
    public void RegisterNewUser_ForExistingEmployeeNotFirstUserAndOwnerCompany_ShouldReturnCompanyWithEmployeeAndUserWithRoleEmployee()
    {
        //arrange
        var company = Company.CreateOwnerCompany(Guid.NewGuid(), "testCompanyName", "test.pl");
        var existedEmployeeId = Guid.NewGuid();
        company.AddEmployee(existedEmployeeId, "first@test.pl");        
        Guid userId = Guid.NewGuid();
        string password = "Test123!";
        _passwordManagerMock
            .Setup(f => f.Secure(It.Is<string>(arg => arg == password)))
            .Returns("securedPassword");
        _userRegistrationService.RegisterNewUser([company], existedEmployeeId,
            userId, "firstName", "lastName", password);

        var newEmployeeId = Guid.NewGuid();
        company.AddEmployee(newEmployeeId, "test@test.pl");
        
        //act
        var result = _userRegistrationService.RegisterNewUser([company], newEmployeeId,
            userId, "firstName", "lastName", password);
        
        //assert
        result.Should().BeOfType<Company>();
        result.Employees.Any(x 
            => x.Id.Equals(newEmployeeId)
               && x.User is not null
               && x.User.Id == userId).Should().BeTrue();
        result.Employees.First(x => x.Id.Equals(newEmployeeId)).User.Role.Should().Be(Role.Employee());
    }
    
    [Fact]
    public void RegisterNewUser_ForNonExistingCompany_ShouldThrowCompanyDoesNotExistException()
    {
        //arrange
        var company = Company.CreateCompany(Guid.NewGuid(), "testCompanyName",
            new TimeSpan(00, 00, 10), "test.pl");

        //act
        var exception = Record.Exception(() => _userRegistrationService.RegisterNewUser([company],  Guid.NewGuid(), 
            Guid.NewGuid(), "firstName", "lastName", "testPass123!"));

        //assert
        exception.Should().BeOfType<CompanyForEmployeeDoesNotExistException>();
    }

    [Fact]
    public void RegisterNewUser_ForExistingUser_ShouldThrowEmailAlreadyExistException()
    {
        //arrange
        var company = Company.CreateCompany(Guid.NewGuid(), "testCompanyName", 
            new TimeSpan(00, 00, 10), "test.pl");
        company.AddEmployee(Guid.NewGuid(), "test@test.pl");
        var employeeId = company.Employees.Single().Id;
        
        string password = "Test123!";
        Guid userId = Guid.NewGuid();
        _passwordManagerMock
            .Setup(f => f.Secure(It.Is<string>(arg => arg == password)))
            .Returns("securedPassword");
        _userRegistrationService.RegisterNewUser([company],  employeeId,
            userId, "firstName", "lastName", password);
        
        //act
        var exception = Record.Exception(() => _userRegistrationService.RegisterNewUser([company],  employeeId,
            userId, "firstName", "lastName", password));
        
        //assert
        exception.Should().BeOfType<UserAlreadyExistsException>();
    }

    [Fact]
    public void RegisterNewUser_ForExistingEmployeeAndTooWeakPassword_ShouldReturnToWeakPasswordException()
    {
        //arrange
        var company = Company.CreateCompany(Guid.NewGuid(), "testCompanyName", 
            new TimeSpan(00, 00, 10), "test.pl");
        company.AddEmployee(Guid.NewGuid(), "test@test.pl");
        var employeeId = company.Employees.Single().Id;
        
        string password = "weakpass";
        Guid userId = Guid.NewGuid();
        _passwordManagerMock
            .Setup(f => f.Secure(It.Is<string>(arg => arg == password)))
            .Returns("securedPassword");
        
        //act
        var exception = Record.Exception(() => _userRegistrationService.RegisterNewUser([company], employeeId,
            userId, "firstName", "lastName", password));
        
        //assert
        exception.Should().BeOfType<ToWeakPasswordException>();
    }
    
    #region arrange
    private readonly IPasswordPolicy _passwordPolicy;
    private readonly Mock<IPasswordManager> _passwordManagerMock;
    private readonly IUserRegistrationService _userRegistrationService;
    
    public UserRegistrationServiceTest()
    {
        _passwordPolicy = new UserPasswordPolicy();
        _passwordManagerMock = new Mock<IPasswordManager>();
        _userRegistrationService = new UserRegistrationService(_passwordManagerMock.Object, _passwordPolicy);
    }
    #endregion
}