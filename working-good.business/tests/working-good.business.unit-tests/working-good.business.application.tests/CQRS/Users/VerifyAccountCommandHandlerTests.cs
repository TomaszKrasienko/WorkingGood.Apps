using FluentAssertions;
using Moq;
using working_good.business.application.CQRS.Users.Command.VerifyAccount;
using working_good.business.application.Exceptions;
using working_good.business.core.Abstractions;
using working_good.business.core.Abstractions.Repositories;
using working_good.business.core.DomainServices;
using working_good.business.core.Models;
using working_good.business.core.Models.Company;
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
        var command = new VerifyAccountCommand(_company.Users.Single().VerificationToken.Token);
        _mockCompanyRepository
            .Setup(f => f.GetByUserVerificationTokenAsync(
                It.Is<string>(arg => arg == command.VerificationToken)))
            .ReturnsAsync(_company);
        
        //act
        await _handler.HandleAsync(command, default);
        
        //assert
        _mockCompanyRepository.Verify(f => f.UpdateAsync(It.Is<Company>(arg => arg == _company)));
        _company.Users.Single().VerificationToken.VerificationDate.Should().NotBeNull();
    }
    
    [Fact]
    public async Task Handle_ForNonExistingUser_ShouldThrowVerificationTokenDoesNotExistsException()
    {
        //arrange
        var command = new VerifyAccountCommand(_company.Users.Single().VerificationToken.Token);
        
        //act
        var exception = await Record.ExceptionAsync(async () => await _handler.HandleAsync(command, default));
        
        //assert
        exception.Should().BeOfType<VerificationTokenDoesNotExistsException>();
    }
    
    #region arrange

    private readonly Mock<ICompanyRepository> _mockCompanyRepository;
    private readonly VerifyAccountCommandHandler _handler;
    private readonly Company _company;
    
    public VerifyAccountCommandHandlerTests()
    {
        _mockCompanyRepository = new Mock<ICompanyRepository>();
        _handler = new VerifyAccountCommandHandler(_mockCompanyRepository.Object);
        var mockPasswordManager = new Mock<IPasswordManager>();
        mockPasswordManager
            .Setup(f => f.Secure(It.IsAny<string>()))
            .Returns("securedPassword");
        _company = Company.CreateOwnerCompany(Guid.NewGuid(), "TestCompany", "test.pl");
        var registrationService = new UserRegistrationService(mockPasswordManager.Object, new UserPasswordPolicy());
        registrationService.RegisterNewUser([_company], _company.Id, Guid.NewGuid(), "test@test.pl",
            "testFirstName", "testLastName", "Test123#", Role.User());
    }
    #endregion
}