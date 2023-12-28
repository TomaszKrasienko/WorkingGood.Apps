using FluentAssertions;
using Moq;
using working_good.business.core.Abstractions;
using working_good.business.core.DomainServices;
using working_good.business.core.DomainServices.Abstractions;
using working_good.business.core.Exceptions;
using working_good.business.core.Models.Company;
using working_good.business.core.Policies;
using working_good.business.core.Policies.Abstractions;
using working_good.business.core.ValueObjects.User;
using Xunit;

namespace working_good.business.core.tests.DomainServices;

public sealed class UserRegistrationServiceTest
{
    [Fact]
    public void RegisterNewUser_ForNonExistingUserAndCorrectEmailDomain_ShouldReturnCompanyWithNewUser()
    {
        //arrnge
        var company = Company.CreateCompany(Guid.NewGuid(), "testCompanyName", 
            new TimeSpan(00, 00, 10), "test.pl");
        string password = "Test123!";
        Guid userId = Guid.NewGuid();
        _passwordManagerMock
            .Setup(f => f.Secure(It.Is<string>(arg => arg == password)))
            .Returns("securedPassword");
        
        //act
        var result = _userRegistrationService.RegisterNewUser([company], company.Id,
            userId, "test@test.pl", "firstName", "lastName",
            password, Role.Employee());
        
        //assert
        result.Should().BeOfType<Company>();
        result.Users.Any(x => x.Id == userId).Should().BeTrue();
    }
    
    [Fact]
    public void RegisterNewUser_ForExistingUser_ShouldThrowEmailAlreadyExistException()
    {
        //arrange
        var existedEmail = "test@test.pl";
        var existedCompany = Company.CreateCompany(Guid.NewGuid(), "testCompanyName", 
            new TimeSpan(00, 00, 10), "test.pl");
        _passwordManagerMock
            .Setup(f => f.Secure(It.Is<string>(arg => arg == "Pass123#")))
            .Returns("securedPassword");
        _userRegistrationService.RegisterNewUser([existedCompany], existedCompany.Id, Guid.NewGuid(), existedEmail,
            "testFirstName", "testLastName", "Pass123#", Role.Employee());
        var company = Company.CreateCompany(Guid.NewGuid(), "testCompanyName", 
            new TimeSpan(00, 00, 10), "test.pl");
        string password = "Test123!";
        Guid userId = Guid.NewGuid();
        _passwordManagerMock
            .Setup(f => f.Secure(It.Is<string>(arg => arg == password)))
            .Returns("securedPassword");
        
        //act
        var exception = Record.Exception(() => _userRegistrationService.RegisterNewUser([company, existedCompany], company.Id,
            userId, existedEmail, "firstName", "lastName",
            password, Role.Employee()));
        
        //assert
        exception.Should().BeOfType<EmailAlreadyExistException>();
    }
    
    [Fact]
    public void RegisterNewUser_ForNonExistingUserAndInvalidEmailDomain_ShouldThrowInvalidUserEmailDomainException()
    {
        //arrange
        var company = Company.CreateCompany(Guid.NewGuid(), "testCompanyName", 
            new TimeSpan(00, 00, 10), "test.pl");
        string password = "Test123!";
        Guid userId = Guid.NewGuid();
        _passwordManagerMock
            .Setup(f => f.Secure(It.Is<string>(arg => arg == password)))
            .Returns("securedPassword");
        
        //act
        var exception = Record.Exception(() => _userRegistrationService.RegisterNewUser(new List<Company>() { company }, company.Id,
            userId, "test@invalidtest.pl", "firstName", "lastName",
            password, Role.Employee()));
        
        //assert
        exception.Should().BeOfType<InvalidUserEmailDomainException>();
    }
    
    [Fact]
    public void RegisterNewUser_ForNonExistingCompany_ShouldThrowCompanyDoesNotExistException()
    {
        //arrange
        var company = Company.CreateCompany(Guid.NewGuid(), "testCompanyName", 
            new TimeSpan(00, 00, 10), "test.pl");
        string password = "Test123!";
        Guid userId = Guid.NewGuid();
        _passwordManagerMock
            .Setup(f => f.Secure(It.Is<string>(arg => arg == password)))
            .Returns("securedPassword");
        
        //act
        var exception = Record.Exception(() => _userRegistrationService.RegisterNewUser(new List<Company>() { company }, Guid.NewGuid(),
            userId, "test@invalidtest.pl", "firstName", "lastName",
            password, Role.Employee()));
        
        //assert
        exception.Should().BeOfType<CompanyDoesNotExistException>();
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