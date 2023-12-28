using FluentAssertions;
using Moq;
using working_good.business.core.Abstractions;
using working_good.business.core.Events;
using working_good.business.core.Models;
using working_good.business.core.Models.Company;
using working_good.business.core.Policies;
using working_good.business.core.Policies.Abstractions;
using working_good.business.core.ValueObjects.User;
using Xunit;

namespace working_good.business.core.tests.Models;

public sealed class CompanyTests
{
    [Fact]
    public void CreateOwnerCompany_ForArguments_ShouldReturnOwnerCompany()
    {
        //arrange
        var id = Guid.NewGuid();
        var companyName = "testCompanyName";
        var emailDomain = "test.pl";
        
        //act
        var result = Company.CreateOwnerCompany(id, companyName, emailDomain);
        
        //assert
        result.Id.Value.Should().Be(id);
        result.Name.Value.Should().Be(companyName);
        result.IsOwner.Value.Should().BeTrue();
        result.SlaTimeSpan.Should().BeNull();
        result.EmailDomain.Value.Should().Be(emailDomain);
    }

    [Fact]
    public void CreateCompany_ForArguments_ShouldReturnCompany()
    {
        //arrange
        var id = Guid.NewGuid();
        var companyName = "testCompanyName";
        var slaTime = new TimeSpan(00, 00, 10);
        var emailDomain = "test.pl";
        //act
        var result = Company.CreateCompany(id, companyName, slaTime, emailDomain);
        
        //assert
        result.Id.Value.Should().Be(id);
        result.Name.Value.Should().Be(companyName);
        result.IsOwner.Value.Should().BeFalse();
        result.SlaTimeSpan.Value.Should().Be(slaTime);
        result.EmailDomain.Value.Should().Be(emailDomain);
    }

    [Fact]
    public void VerifyUser_ForValidToken_ShouldActivateUser()
    {
        //arrange
        var company = Company.CreateCompany(Guid.NewGuid(), "testCompanyName", 
            new TimeSpan(00, 00, 10), "test.pl");
        string password = "Test123!";
        Guid userId = Guid.NewGuid();
        string email = "test@test.pl";
        FullName fullName = new FullName("firstName", "lastName");
        _passwordManagerMock
            .Setup(f => f.Secure(It.Is<string>(arg => arg == password)))
            .Returns("securedPassword");
        var user = company.RegisterUser(_passwordPolicy, _passwordManagerMock.Object, userId, email, fullName,
            password, Role.User());
        
        //act
        company.VerifyUser(user.VerificationToken.Token);
        
        //assert
        user.VerificationToken.VerificationDate.Should().NotBeNull();
    }

    [Fact]
    public void CanUserBeLogged_ForActiveUser_ShouldReturnTrue()
    {
        //arrange
        var company = Company.CreateCompany(Guid.NewGuid(), "testCompanyName", 
            new TimeSpan(00, 00, 10), "test.pl");
        string password = "Test123!";
        Guid userId = Guid.NewGuid();
        string email = "test@test.pl";
        FullName fullName = new FullName("firstName", "lastName");
        _passwordManagerMock
            .Setup(f => f.Secure(It.Is<string>(arg => arg == password)))
            .Returns("securedPassword");
        var user = company.RegisterUser(_passwordPolicy, _passwordManagerMock.Object, userId, email, fullName,
            password, Role.User());
        company.VerifyUser(user.VerificationToken.Token);
        
        //act
        var result = company.CanUserBeLogged(email);
        
        //Assert
        result.Should().BeTrue();
    }
    
    [Fact]
    public void CanUserBeLogged_ForNoActiveUser_ShouldReturnFalse()
    {
        //arrange
        var company = Company.CreateCompany(Guid.NewGuid(), "testCompanyName", 
            new TimeSpan(00, 00, 10), "test.pl");
        string password = "Test123!";
        Guid userId = Guid.NewGuid();
        string email = "test@test.pl";
        FullName fullName = new FullName("firstName", "lastName");
        _passwordManagerMock
            .Setup(f => f.Secure(It.Is<string>(arg => arg == password)))
            .Returns("securedPassword");
        var user = company.RegisterUser(_passwordPolicy, _passwordManagerMock.Object, userId, email, fullName,
            password, Role.User());
        
        //act
        var result = company.CanUserBeLogged(email);
        
        //Assert
        result.Should().BeFalse();
    }
    
    #region arrange

    private readonly IPasswordPolicy _passwordPolicy;
    private readonly Mock<IPasswordManager> _passwordManagerMock;
    
    public CompanyTests()
    {
        _passwordPolicy = new UserPasswordPolicy();
        _passwordManagerMock = new Mock<IPasswordManager>();
    }
    #endregion
}