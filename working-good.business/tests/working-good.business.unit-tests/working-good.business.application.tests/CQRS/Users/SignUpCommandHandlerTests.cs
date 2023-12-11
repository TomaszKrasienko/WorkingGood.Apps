using FluentAssertions;
using Moq;
using working_good.business.application.CQRS.Users.Command.SignUp;
using working_good.business.core.Abstractions;
using working_good.business.core.Abstractions.Repositories;
using working_good.business.core.Models;
using working_good.business.core.Policies;
using working_good.business.core.Policies.Abstractions;
using working_good.business.core.ValueObjects.User;
using Xunit;

namespace working_good.business.application.tests.CQRS.Users;

public sealed class SignUpCommandHandlerTests
{
    [Fact]
    public async Task SignUp_ForSignUpCommand_ShouldAddByUserRepositoryAndSendByEventProcessor()
    {
        //arrange
        SignUpCommand command = new SignUpCommand(Guid.NewGuid(), "test@test.pl", "testFirstName",
            "testLastName", "StrongPass123!", "User");
        _userRepositoryMock
            .Setup(f => f.GetByEmailAsync(It.Is<string>(arg => arg == command.Email)));
        _passwordManagerMock
            .Setup(f => f.Secure(It.Is<string>(arg => arg == command.Password)))
            .Returns("NewStrongSecuredPassword");
        
        //act
        await _handler.HandleAsync(command, default);
        
        //assert
        _userRepositoryMock
            .Verify(f => f.AddAsync(It.Is<User>(arg 
                => arg.Email == command.Email
                && arg.FullName.FirstName == command.FirstName
                && arg.FullName.LastName == command.LastName
                && arg.Id == command.Id)));
    }
    
    [Fact]
    public async Task SignUp_ForSignUpCommandAndExistingUser_ShouldThrowEmailAlreadyExistException()
    {
        //arrange
        SignUpCommand command = new SignUpCommand(Guid.NewGuid(), "test@test.pl", "testFirstName",
            "testLastName", "StrongPass123!", "User");        
        _passwordManagerMock
            .Setup(f => f.Secure(It.Is<string>(arg => arg == "strongPassword123!")))
            .Returns("NewStrongSecuredPassword");
        _userRepositoryMock
            .Setup(f => f.GetByEmailAsync(It.Is<string>(arg => arg == command.Email)))
            .ReturnsAsync(User.CreateUser(_passwordPolicy,_passwordManagerMock.Object, Guid.NewGuid(), command.Email, new FullName("test", "test"),
                "strongPassword123!", Role.User()));

        //act
        var exception = await Record.ExceptionAsync(async() => await _handler.HandleAsync(command, default));
        
        //assert
        exception.Should().BeOfType<EmailAlreadyExistException>();
    }
    
    #region arrange

    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IPasswordManager> _passwordManagerMock;
    private readonly IPasswordPolicy _passwordPolicy;
    private readonly SignUpCommandHandler _handler;
    
    public SignUpCommandHandlerTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _passwordManagerMock = new Mock<IPasswordManager>();
        _passwordPolicy = new UserPasswordPolicy();
        _handler = new SignUpCommandHandler(_userRepositoryMock.Object, _passwordManagerMock.Object, _passwordPolicy);
    }
    #endregion
}