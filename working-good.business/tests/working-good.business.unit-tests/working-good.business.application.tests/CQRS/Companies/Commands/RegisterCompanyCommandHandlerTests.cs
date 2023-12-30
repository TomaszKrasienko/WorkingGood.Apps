using Moq;
using working_good.business.application.CQRS.Companies.Commands.Register;
using working_good.business.core.Abstractions.Repositories;
using working_good.business.core.DomainServices;
using working_good.business.core.DomainServices.Abstractions;
using working_good.business.core.Models.Company;
using Xunit;

namespace working_good.business.application.tests.CQRS.Companies.Commands;

public sealed class RegisterCompanyCommandHandlerTests
{
    [Fact]
    public async Task Handle_ForRegisterCompanyCommandWithOwnerField_ShouldAddCompanyByRepository()
    {
        //arrange
        _companyRepositoryMock
            .Setup(f => f.GetAllAsync());
        var command = new RegisterCompanyCommand(Guid.NewGuid(), "testOwnerCompanyName", true,
            "test.pl");

        //act
        await _handler.HandleAsync(command, default);
        
        //assert
        _companyRepositoryMock
            .Verify(f => f.AddAsync(It.Is<Company>(arg
                => arg.Id == command.Id
                && arg.Name == command.Name
                && arg.IsOwner == command.IsOwner
                && arg.EmailDomain == command.EmailDomain)));
    }
    
    [Fact]
    public async Task Handle_ForRegisterCompanyCommandWithExistedCompanies_ShouldAddCompanyByRepository()
    {
        //arrange
        List<Company> companies = new List<Company>()
        {
            _company,
            _ownerCompany
        };
        _companyRepositoryMock
            .Setup(f => f.GetAllAsync())
            .ReturnsAsync(companies);
        var command = new RegisterCompanyCommand(Guid.NewGuid(), "newCompany", false,
            "newtest.pl", TimeSpan.FromDays(1));

        //act
        await _handler.HandleAsync(command, default);
        
        //assert
        _companyRepositoryMock
            .Verify(f => f.AddAsync(It.Is<Company>(arg
                => arg.Id == command.Id
                   && arg.Name == command.Name
                   && arg.IsOwner == command.IsOwner
                   && arg.EmailDomain == command.EmailDomain
                   && arg.SlaTimeSpan.Value == command.SlaTimeSpan)));
    }
    
    #region arrange
    private readonly Mock<ICompanyRepository> _companyRepositoryMock;
    private readonly ICompanyRegistrationService _companyRegistrationService;
    private readonly RegisterCompanyCommandHandler _handler;
    
    private readonly Company _company;
    private readonly Company _ownerCompany;
    public RegisterCompanyCommandHandlerTests()
    {
        _companyRepositoryMock = new Mock<ICompanyRepository>();
        _companyRegistrationService = new CompanyRegistrationService();
        _handler = new RegisterCompanyCommandHandler(_companyRegistrationService, _companyRepositoryMock.Object);
        _company = Company.CreateCompany(Guid.NewGuid(), "testCompanyName", 
            new TimeSpan(10,10,10), "test.pl");
        _ownerCompany = Company.CreateOwnerCompany(Guid.NewGuid(), "testOwnerCompanyName",
            "owner.pl");
    }

    #endregion
}