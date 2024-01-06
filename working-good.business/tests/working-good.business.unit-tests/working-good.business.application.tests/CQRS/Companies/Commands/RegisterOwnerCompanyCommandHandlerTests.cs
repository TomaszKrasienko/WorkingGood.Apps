using Moq;
using working_good.business.application.CQRS.Companies.Commands.RegisterOwnerCompany;
using working_good.business.application.CQRS.Employees.Commands;
using working_good.business.core.Abstractions;
using working_good.business.core.Abstractions.Repositories;
using working_good.business.core.DomainServices;
using working_good.business.core.DomainServices.Abstractions;
using working_good.business.core.Models.Company;
using working_good.business.core.Policies;
using Xunit;

namespace working_good.business.application.tests.CQRS.Companies.Commands;

public sealed class RegisterOwnerCompanyCommandHandlerTests
{
    [Fact]
    public async Task
        Handle_ForRegisterOwnerCompanyAndNotExistedCompanies_ShouldAddCompanyWithEmployeeAndUserByRepository()
    {
        //arrange
        var command = new RegisterOwnerCompanyCommand(Guid.NewGuid(), "testOwnerName", "outlook.com",
            Guid.NewGuid(), "test@outlook.com", Guid.NewGuid(), "testFirstName",
            "testLastName", "Pass123!");
        _passwordManagerMock
            .Setup(f => f.Secure(It.Is<string>(arg => arg == command.Password)))
            .Returns("StrongPass123!");
        
        //act
        await _handler.HandleAsync(command, default);
        
        //assert
        _companyRepositoryMock
            .Verify(f => f.AddAsync(It.Is<Company>(arg 
                => arg.Id == command.CompanyId
                && arg.Name == command.Name
                && arg.IsOwner == true
                && arg.Employees.Single(x => x.Id == command.EmployeeId).Email == command.EmployeeEmail
                && arg.Employees.Single(x => x.Id == command.EmployeeId).User.Id == command.UserId)));
    }
    
    #region arrange
    private readonly ICompanyRegistrationService _companyRegistrationService;
    private readonly Mock<ICompanyRepository> _companyRepositoryMock; 
    private readonly IUserRegistrationService _userRegistrationService;
    private readonly Mock<IPasswordManager> _passwordManagerMock;
    private readonly RegisterOwnerCompanyCommandHandler _handler;

    public RegisterOwnerCompanyCommandHandlerTests()
    {
        _companyRegistrationService = new CompanyRegistrationService();
        _companyRepositoryMock = new Mock<ICompanyRepository>();
        _passwordManagerMock = new Mock<IPasswordManager>();
        _userRegistrationService = new UserRegistrationService(_passwordManagerMock.Object, new UserPasswordPolicy());
        _handler = new RegisterOwnerCompanyCommandHandler(_companyRegistrationService, _companyRepositoryMock.Object,
            _userRegistrationService);
    }
    #endregion
}