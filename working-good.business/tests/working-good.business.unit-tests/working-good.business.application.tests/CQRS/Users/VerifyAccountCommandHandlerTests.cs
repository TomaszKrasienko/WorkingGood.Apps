using FluentAssertions;
using Moq;
using working_good.business.application.CQRS.Users.Command.VerifyAccount;
using working_good.business.application.Exceptions;
using working_good.business.core.Abstractions;
using working_good.business.core.Abstractions.Repositories;
using working_good.business.core.Models;
using working_good.business.core.Policies;
using working_good.business.core.ValueObjects.User;
using Xunit;

namespace working_good.business.application.tests.CQRS.Users;

public sealed class VerifyAccountCommandHandlerTests
{
    [Fact]
    public async Task Handle_ForValidVerifyAccountCommand_ShouldUpdateUserVerificationToken()
    {
        //arrange
        var command = new VerifyAccountCommand(_user.VerificationToken.Token);
        _mockUserRepository
            .Setup(f => f.GetByVerificationToken(
                It.Is<string>(arg => arg == command.VerificationToken)))
            .ReturnsAsync(_user);
        
        //act
        await _handler.HandleAsync(command, default);
        
        //assert
        _user.VerificationToken.VerificationDate.Should().NotBeNull();
    }
    
    [Fact]
    public async Task Handle_ForNonExistingUser_ShouldThrowVerificationTokenDoesNotExistsException()
    {
        //arrange
        var command = new VerifyAccountCommand(_user.VerificationToken.Token);
        
        //act
        var exception = await Record.ExceptionAsync(async () => await _handler.HandleAsync(command, default));
        
        //assert
        exception.Should().BeOfType<VerificationTokenDoesNotExistsException>();
    }
    
    #region arrange

    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly VerifyAccountCommandHandler _handler;
    private readonly User _user;
    
    public VerifyAccountCommandHandlerTests()
    {
        _mockUserRepository = new Mock<IUserRepository>();
        _handler = new VerifyAccountCommandHandler(_mockUserRepository.Object);
        var mockPasswordManager = new Mock<IPasswordManager>();
        mockPasswordManager
            .Setup(f => f.Secure(It.IsAny<string>()))
            .Returns("securedPassword");
        _user = User.CreateUser(new UserPasswordPolicy(), mockPasswordManager.Object, Guid.NewGuid(), "test@test.pl",
            new FullName("testFirstName", "testLastName"), "Test123#", Role.User());
    }
    #endregion
}