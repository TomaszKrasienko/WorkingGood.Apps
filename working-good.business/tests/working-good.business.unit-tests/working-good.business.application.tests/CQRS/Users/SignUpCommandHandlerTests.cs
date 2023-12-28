using FluentAssertions;
using Moq;
using working_good.business.application.CQRS.Users.Command.SignUp;
using working_good.business.core.Abstractions;
using working_good.business.core.Abstractions.Repositories;
using working_good.business.core.DomainServices;
using working_good.business.core.DomainServices.Abstractions;
using working_good.business.core.Models.Company;
using working_good.business.core.Policies;
using working_good.business.core.Policies.Abstractions;
using Xunit;

namespace working_good.business.application.tests.CQRS.Users;

public sealed class SignUpCommandHandlerTests
{
    [Fact]
    public async Task SignUp_ForSignUpCommandAndExistingCompany_ShouldAddToCompanyAndUpdateCompany()
    {
        //arrange
        Company company = Company.CreateOwnerCompany(Guid.NewGuid(), "TestCompany", "test.pl");
        SignUpCommand command = new SignUpCommand(company.Id, Guid.NewGuid(), "test@test.pl", "testFirstName",
            "testLastName", "StrongPass123!", "User");
        _passwordManagerMock
            .Setup(f => f.Secure(It.Is<string>(arg => arg == command.Password)))
            .Returns("NewStrongSecuredPassword");
        _companyRepositoryMock
            .Setup(f => f.GetAllAsync())
            .ReturnsAsync([company]);
            
        //act
        await _handler.HandleAsync(command, default);
        
        //assert
        company.Users.Any().Should().BeTrue();
        _companyRepositoryMock
            .Verify(f => f.UpdateAsync(company));
    }
    
    [Fact]
    public async Task SignUp_ForSignUpCommandAndNonExistingCompanies_ShouldThrowCompaniesDoesNotExistsException()
    {
        //arrange
        SignUpCommand command = new SignUpCommand(Guid.NewGuid(), Guid.NewGuid(), "test@test.pl", 
            "testFirstName", "testLastName", "StrongPass123!", "User");        
        _passwordManagerMock
            .Setup(f => f.Secure(It.Is<string>(arg => arg == "strongPassword123!")))
            .Returns("NewStrongSecuredPassword");
    
        //act
        var exception = await Record.ExceptionAsync(async() => await _handler.HandleAsync(command, default));
        
        //assert
        exception.Should().BeOfType<CompaniesDoesNotExistException>();
    }
    
    #region arrange

    private readonly Mock<ICompanyRepository> _companyRepositoryMock;
    private readonly Mock<IPasswordManager> _passwordManagerMock;
    private readonly IPasswordPolicy _passwordPolicy;
    private readonly IUserRegistrationService _userRegistrationService;
    private readonly SignUpCommandHandler _handler;
    
    public SignUpCommandHandlerTests()
    {
        _companyRepositoryMock = new Mock<ICompanyRepository>();
        _passwordManagerMock = new Mock<IPasswordManager>();
        _passwordPolicy = new UserPasswordPolicy();
        _userRegistrationService = new UserRegistrationService(_passwordManagerMock.Object, _passwordPolicy);
        _handler = new SignUpCommandHandler(_companyRepositoryMock.Object, _userRegistrationService);
    }
    #endregion
}