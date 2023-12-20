using FluentAssertions;
using Moq;
using working_good.business.core.Abstractions;
using working_good.business.core.Events;
using working_good.business.core.Exceptions;
using working_good.business.core.Models;
using working_good.business.core.Policies;
using working_good.business.core.Policies.Abstractions;
using working_good.business.core.ValueObjects.User;
using Xunit;

namespace working_good.business.core.tests.Models;

public sealed class UserTests
{
    [Fact]
    public void Create_ForValid_ShouldReturnUserObject()
    {
        //Arrange
        string password = "Test123!";
        Guid userId = Guid.NewGuid();
        string email = "test@test.pl";
        FullName fullName = new FullName("firstName", "lastName");
        _passwordManagerMock
            .Setup(f => f.Secure(It.Is<string>(arg => arg == password)))
            .Returns("securedPassword");
        
        //Act
        var result = User.CreateUser(_passwordPolicy, _passwordManagerMock.Object, userId, email, fullName,
            password, Role.User());
        
        //Assert
        result.Should().NotBeNull();
        result.Id.Value.Should().Be(userId);
        result.Email.Value.Should().Be(email);
        result.FullName.Should().BeEquivalentTo(fullName);
        result.Password.Value.Should().NotBeNullOrWhiteSpace();
        result.Password.Value.Should().NotBe(password);
        result.Events.Any(x => x.GetType() == typeof(UserAdded)).Should().BeTrue();
    }

    [Theory]
    [InlineData("test")]
    [InlineData("test.pl")]
    [InlineData("test@.pl")]
    public void Create_ForInvalidEmailAddress_ShouldThrowInvalidEmailException(string email)
    {
        //Arrange
        string password = "Test123!";
        _passwordManagerMock
            .Setup(f => f.Secure(It.Is<string>(arg => arg == password)))
            .Returns("securedPassword");
        
        //Act
        var exception = Record.Exception(() => User.CreateUser(_passwordPolicy, 
            _passwordManagerMock.Object, Guid.NewGuid(), email, new FullName("firstName", "lastName"),password, Role.User()));
        
        //Assert
        exception.Should().BeOfType<InvalidEmailException>();
    }

    [Theory]
    [InlineData("test", "")]
    [InlineData("", "test")]
    public void Create_ForInvalidFullName_ShouldThrowInvalidFullNameException(string firstName, string lastName)
    {
        //Arrange
        string password = "Test123!";
        _passwordManagerMock
            .Setup(f => f.Secure(It.Is<string>(arg => arg == password)))
            .Returns("securedPassword");
        
        //Act
        var exception = Record.Exception(() => User.CreateUser(_passwordPolicy, _passwordManagerMock.Object, Guid.NewGuid(), "test@test.pl", 
            new FullName(firstName, lastName), password, Role.User()));
        
        //Assert
        exception.Should().BeOfType<InvalidFullNameException>();
    }

    [Fact]
    public void Create_ForNonExistingRole_ShouldThrowInvalidUserRoleException()
    {
        //Arrange
        string password = "Test123!";
        _passwordManagerMock
            .Setup(f => f.Secure(It.Is<string>(arg => arg == password)))
            .Returns("securedPassword");
        
        //Act
        var exception = Record.Exception(() => User.CreateUser(_passwordPolicy, _passwordManagerMock.Object, Guid.NewGuid(), "test@test.pl", 
            new FullName("firstName", "lastName"), password, "Non existing role"));
        
        //Assert
        exception.Should().BeOfType<InvalidUserRoleException>();
    }

    [Theory]
    [InlineData("Testtest")]
    [InlineData("test123")]
    [InlineData("TEST123")]
    [InlineData("testTEST")]
    public void Create_ForToWeakPassword_ShouldThrowToWeakPasswordException(string password)
    {     
        //Arrange
        _passwordManagerMock
            .Setup(f => f.Secure(It.Is<string>(arg => arg == password)))
            .Returns("securedPassword");
        
        //Act
        var exception = Record.Exception(() => User.CreateUser(_passwordPolicy, _passwordManagerMock.Object, Guid.NewGuid(), "test@test.pl", 
            new FullName("firstName", "lastName"), password, Role.Employee()));
        
        //Assert
        exception.Should().BeOfType<ToWeakPasswordException>();
    }

    [Fact]
    public void VerifyAccount_ForValidVerificationToken_ShouldSetVerificationDate()
    {
        //Arrange
        _passwordManagerMock
            .Setup(f => f.Secure(It.IsAny<string>()))
            .Returns("securedPassword");
        var user = User.CreateUser(_passwordPolicy, _passwordManagerMock.Object, Guid.NewGuid(),
            "test@test.pl", new FullName("testFirstName", "testLastName"), "Test123!", Role.Employee());
        
        //Act
        user.VerifyAccount(user.VerificationToken.Token);
        
        //Assert
        user.VerificationToken.VerificationDate.Should().NotBeNull();
    }
    
    [Fact]
    public void VerifyAccount_ForInvalidVerificationToken_ShouldThrowInvalidAccountVerificationException()
    {
        //Arrange
        _passwordManagerMock
            .Setup(f => f.Secure(It.IsAny<string>()))
            .Returns("securedPassword");
        var user = User.CreateUser(_passwordPolicy, _passwordManagerMock.Object, Guid.NewGuid(),
            "test@test.pl", new FullName("testFirstName", "testLastName"), "Test123!", Role.Employee());
        
        //Act
        var exception = Record.Exception(() => user.VerifyAccount("testInvalidToken"));
        
        //Assert
        exception.Should().BeOfType<InvalidAccountVerificationException>();
    }

    [Fact]
    public void CanBeLogged_ForActiveUser_ShouldReturnTrue()
    {
        //arrange
        _passwordManagerMock
            .Setup(f => f.Secure(It.IsAny<string>()))
            .Returns("securedPassword");
        var user = User.CreateUser(_passwordPolicy, _passwordManagerMock.Object, Guid.NewGuid(),
            "test@test.pl", new FullName("testFirstName", "testLastName"), "Test123!", Role.Employee());
        user.VerifyAccount(user.VerificationToken.Token);
        
        //act
        var result = user.CanBeLogged();
        
        //assert
        result.Should().BeTrue();
    }
    
    [Fact]
    public void CanBeLogged_ForNonActiveUser_ShouldReturnFalse()
    {
        //arrange
        _passwordManagerMock
            .Setup(f => f.Secure(It.IsAny<string>()))
            .Returns("securedPassword");
        var user = User.CreateUser(_passwordPolicy, _passwordManagerMock.Object, Guid.NewGuid(),
            "test@test.pl", new FullName("testFirstName", "testLastName"), "Test123!", Role.Employee());
        
        //act
        var result = user.CanBeLogged();
        
        //assert
        result.Should().BeFalse();
    }
    
    #region arrange

    private readonly IPasswordPolicy _passwordPolicy;
    private readonly Mock<IPasswordManager> _passwordManagerMock;
    
    public UserTests()
    {
        _passwordPolicy = new UserPasswordPolicy();
        _passwordManagerMock = new Mock<IPasswordManager>();
    }
    #endregion
    
}