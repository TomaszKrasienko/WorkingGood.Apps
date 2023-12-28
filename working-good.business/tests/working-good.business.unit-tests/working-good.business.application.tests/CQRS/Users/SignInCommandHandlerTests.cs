using FluentAssertions;
using Moq;
using working_good.business.application.CQRS.Users.Command.SignIn;
using working_good.business.application.CQRS.Users.Command.VerifyAccount;
using working_good.business.application.DTOs;
using working_good.business.application.Exceptions;
using working_good.business.application.Services;
using working_good.business.core.Abstractions;
using working_good.business.core.Abstractions.Repositories;
using working_good.business.core.DomainServices;
using working_good.business.core.Models;
using working_good.business.core.Models.Company;
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
        var command = new SignInCommand(_company.Users.Single().Email.Value, _userPassword);
        _mockCompanyRepository
            .Setup(f => f.GetByUserEmailAsync(It.Is<string>(arg => arg == command.Email)))
            .ReturnsAsync(_company);
        _mockAuthenticator
            .Setup(f => f.CreateAccessToken(_company.Users.Single().Id, new List<string>()
            {
                _company.Users.Single().Role
            }))
            .Returns(new AccessTokenDto(token));
        _company.VerifyUser(_company.Users.Single().VerificationToken.Token);
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
        var command = new SignInCommand(_company.Users.Single().Email.Value, _userPassword);
        _mockCompanyRepository
            .Setup(f => f.GetByUserEmailAsync(It.Is<string>(arg => arg == command.Email)));
        _mockAuthenticator
            .Setup(f => f.CreateAccessToken(_company.Users.Single().Id, new List<string>()
            {
                _company.Users.Single().Role
            }))
            .Returns(new AccessTokenDto(token));
        
        //act
        var exception = await Record.ExceptionAsync(async() 
            => await _handler.HandleAsync(command, default));
        
        //assert
        exception.Should().BeOfType<UserNotFoundException>();
    }
    
    [Fact]
    public async Task Handle_ForValidCredentialsAndFalseCanBeLoggedUser_ShouldThrowUserCanNotBeLoggedException()
    {
        //arrange
        string token = "newAccessToken";
        var command = new SignInCommand(_company.Users.Single().Email.Value, _userPassword);
        _mockCompanyRepository
            .Setup(f => f.GetByUserEmailAsync(It.Is<string>(arg => arg == command.Email)))
            .ReturnsAsync(_company);
        _mockAuthenticator
            .Setup(f => f.CreateAccessToken(_company.Users.Single().Id, new List<string>()
            {
                _company.Users.Single().Role
            }))
            .Returns(new AccessTokenDto(token));
        
        //act
        var exception = await Record.ExceptionAsync(async() 
            => await _handler.HandleAsync(command, default));
        
        //assert
        exception.Should().BeOfType<UserCanNotBeLoggedException>();
    }
    
    [Fact]
    public async Task Handle_ForInvalidPassword_ShouldThrowIncorrectPasswordException()
    {
        //arrange
        string token = "newAccessToken";
        var command = new SignInCommand(_company.Users.Single().Email.Value, "RandomStrongPassword");
        _mockCompanyRepository
            .Setup(f => f.GetByUserEmailAsync(It.Is<string>(arg => arg == command.Email)))
            .ReturnsAsync(_company);
        _mockAuthenticator
            .Setup(f => f.CreateAccessToken(_company.Users.Single().Id, new List<string>()
            {
                _company.Users.Single().Role
            }))
            .Returns(new AccessTokenDto(token));
        _company.VerifyUser(_company.Users.Single().VerificationToken.Token);
        
        //act
        var exception = await Record.ExceptionAsync(async() 
            => await _handler.HandleAsync(command, default));
        
        //assert
        exception.Should().BeOfType<IncorrectPasswordException>();
    }
    
    #region arrange
    
    private readonly Mock<ICompanyRepository> _mockCompanyRepository;
    private readonly Mock<IPasswordManager> _mockPasswordManager;
    private readonly Mock<IAuthenticator> _mockAuthenticator;
    private readonly Mock<IAccessTokenStorage> _mockAccessTokenStorage;
    private readonly SignInCommandHandler _handler;
    private const string _userPassword = "Test123!";
    private readonly Company _company;
    
    public SignInCommandHandlerTests()
    {
        _mockCompanyRepository = new Mock<ICompanyRepository>();
        _mockPasswordManager = new Mock<IPasswordManager>();
        _mockAuthenticator = new Mock<IAuthenticator>();
        _mockAccessTokenStorage = new Mock<IAccessTokenStorage>();
        _handler = new SignInCommandHandler(_mockCompanyRepository.Object, _mockAuthenticator.Object, 
            _mockAccessTokenStorage.Object, _mockPasswordManager.Object);
        _mockPasswordManager
            .Setup(f => f.Secure(It.IsAny<string>()))
            .Returns("securedPassword");
        _mockPasswordManager
            .Setup(f => f.IsValidPassword(
                It.Is<string>(arg => arg == _userPassword),
                It.IsAny<string>()))
            .Returns(true);
        _company = Company.CreateOwnerCompany(Guid.NewGuid(), "TestCompany", "test.pl");
        var registrationService = new UserRegistrationService(_mockPasswordManager.Object, new UserPasswordPolicy());
        registrationService.RegisterNewUser([_company], _company.Id, Guid.NewGuid(), "test@test.pl",
            "testFirstName", "testLastName", "Test123#", Role.User());
    }
    #endregion
}