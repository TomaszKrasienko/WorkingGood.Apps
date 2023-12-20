using FluentAssertions;
using Moq;
using working_good.business.application.CQRS.Users.Command.SignIn;
using working_good.business.application.CQRS.Users.Command.VerifyAccount;
using working_good.business.application.DTOs;
using working_good.business.application.Exceptions;
using working_good.business.application.Services;
using working_good.business.core.Abstractions;
using working_good.business.core.Abstractions.Repositories;
using working_good.business.core.Models;
using working_good.business.core.Policies;
using working_good.business.core.ValueObjects.User;
using Xunit;

namespace working_good.business.application.tests.CQRS.Users;

public sealed class SignInCommandHandlerTests
{
    [Fact]
    public async Task Handle_ForValidCredentialsAndActiveUser_ShouldSetTokenToStorage()
    {
        //arrange
        string token = "newAccessToken";
        var command = new SignInCommand(_user.Email.Value, _userPassword);
        _mockUserRepository
            .Setup(f => f.GetByEmailAsync(It.Is<string>(arg => arg == command.Email)))
            .ReturnsAsync(_user);
        _mockAuthenticator
            .Setup(f => f.CreateAccessToken(_user.Id, new List<string>()
            {
                _user.Role
            }))
            .Returns(new AccessTokenDto(token));
        
        //act
        await _handler.HandleAsync(command, default);
        
        //assert
        _mockAccessTokenStorage
            .Verify(f => f.Set(It.Is<AccessTokenDto>(arg 
                => arg.Token == token)));
    }
    
    [Fact]
    public async Task Handle_ForNonExistingUser_ShouldThrowUserNotFoundException()
    {
        //arrange
        string token = "newAccessToken";
        var command = new SignInCommand(_user.Email.Value, _userPassword);
        _mockUserRepository
            .Setup(f => f.GetByEmailAsync(It.Is<string>(arg => arg == command.Email)));
        _mockAuthenticator
            .Setup(f => f.CreateAccessToken(_user.Id, new List<string>()
            {
                _user.Role
            }))
            .Returns(new AccessTokenDto(token));
        
        //act
        var exception = await Record.ExceptionAsync(async() 
            => await _handler.HandleAsync(command, default));
        
        //assert
        exception.Should().BeOfType<UserNotFoundException>();
    }
    
    [Fact]
    public async Task Handle_ForValidCredentialsAndNonActiveUser_ShouldThrowUserNotActiveException()
    {
        //arrange
        string token = "newAccessToken";
        var command = new SignInCommand(_user.Email.Value, _userPassword);
        _mockUserRepository
            .Setup(f => f.GetByEmailAsync(It.Is<string>(arg => arg == command.Email)))
            .ReturnsAsync(_user);
        _mockAuthenticator
            .Setup(f => f.CreateAccessToken(_user.Id, new List<string>()
            {
                _user.Role
            }))
            .Returns(new AccessTokenDto(token));
        
        //act
        var exception = await Record.ExceptionAsync(async() 
            => await _handler.HandleAsync(command, default));
        
        //assert
        exception.Should().BeOfType<UserNotActiveException>();
    }
    
    #region arrange
    
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly Mock<IPasswordManager> _mockPasswordManager;
    private readonly Mock<IAuthenticator> _mockAuthenticator;
    private readonly Mock<IAccessTokenStorage> _mockAccessTokenStorage;
    private readonly SignInCommandHandler _handler;
    private const string _userPassword = "Test123!";
    private readonly User _user;
    
    public SignInCommandHandlerTests()
    {
        _mockUserRepository = new Mock<IUserRepository>();
        _mockPasswordManager = new Mock<IPasswordManager>();
        _mockAuthenticator = new Mock<IAuthenticator>();
        _mockAccessTokenStorage = new Mock<IAccessTokenStorage>();
        _handler = new SignInCommandHandler(_mockUserRepository.Object, _mockAuthenticator.Object, _mockAccessTokenStorage.Object);
        _mockPasswordManager
            .Setup(f => f.Secure(It.IsAny<string>()))
            .Returns("securedPassword");
        _mockPasswordManager
            .Setup(f => f.IsValidPassword(
                It.Is<string>(arg => arg == _userPassword),
                It.IsAny<string>()))
            .Returns(true);
        _user = User.CreateUser(new UserPasswordPolicy(), _mockPasswordManager.Object, Guid.NewGuid(), "test@test.pl",
            new FullName("testFirstName", "testLastName"), _userPassword, Role.User());
    }
    #endregion
}