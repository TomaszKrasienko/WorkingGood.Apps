using FluentAssertions;
using Moq;
using working_good.business.core.Abstractions;
using working_good.business.core.DomainServices;
using working_good.business.core.DomainServices.Abstractions;
using working_good.business.core.Exceptions;
using working_good.business.core.Models.Company;
using working_good.business.core.Policies;
using working_good.business.core.Policies.Abstractions;
using working_good.business.core.ValueObjects.User;
using Xunit;

namespace working_good.business.core.tests.DomainServices;

public sealed class CompanyRegistrationServiceTests
{
    [Fact]
    public void RegisterCompany_ForCompanyWithAnotherExistedCompanyAndOwnerCompany_ShouldReturnCompany()
    {
        //arrange
        var employeeId = Guid.NewGuid();
        _ownerCompany.AddEmployee(employeeId, "test@owner.pl");
        _ownerCompany.RegisterUser(_passwordPolicy, _passwordManagerMock.Object, 
            employeeId, Guid.NewGuid(), new FullName("test", "test"), "Pass123!");
        List<Company> companies = new List<Company>()
        {
            _company,
            _ownerCompany
        };
        
        //act
        var result = _companyRegistrationService.RegisterCompany(companies, Guid.NewGuid(), "newCompany", false,
             "new.pl", new TimeSpan(10000));
        
        //assert
        result.Should().BeOfType<Company>();
    }
    
    [Fact]
    public void RegisterCompany_ForOwnerCompanyAndNullCompaniesList_ShouldReturnCompany()
    {
        //act
        var result = _companyRegistrationService.RegisterCompany(null, Guid.NewGuid(), "newCompany", true,
             "new.pl");
        
        //assert
        result.Should().BeOfType<Company>();
    }
    
    [Fact]
    public void RegisterCompany_ForCompanyAndNullCompaniesList_ShouldThrowOwnerCompanyDoesNotExistsException()
    {
        //act
        var exception = Record.Exception(() => _companyRegistrationService.RegisterCompany(null, Guid.NewGuid(), "newCompany", false,
            "new.pl", new TimeSpan(10000)));
        
        //assert
        exception.Should().BeOfType<OwnerCompanyDoesNotExistsException>();
    }
    
    [Fact]
    public void RegisterCompany_ForCompanyAndOwnerCompanyWithoutEmployee_ShouldThrowOwnerCompanyDoesNotExistsException()
    {
        //arrange
        List<Company> companies = new List<Company>()
        {
            _ownerCompany
        };
        
        //act
        var exception = Record.Exception(() => _companyRegistrationService.RegisterCompany(companies, Guid.NewGuid(), "newCompany", false,
            "new.pl", new TimeSpan(10000)));
        
        //assert
        exception.Should().BeOfType<OwnerCompanyDoesNotExistsException>();
    }
    
    [Fact]
    public void RegisterCompany_ForCompanyAndOwnerCompanyWithEmployeeAndWithoutUser_ShouldThrowOwnerCompanyDoesNotExistsException()
    {
        //arrange
        var employeeId = Guid.NewGuid();
        _ownerCompany.AddEmployee(employeeId, "test@owner.pl");
        List<Company> companies = new List<Company>()
        {
            _ownerCompany
        };
        //act
        var exception = Record.Exception(() => _companyRegistrationService.RegisterCompany(companies, Guid.NewGuid(), "newCompany", false,
            "new.pl", new TimeSpan(10000)));
        
        //assert
        exception.Should().BeOfType<OwnerCompanyDoesNotExistsException>();
    }
    
    [Fact]
    public void RegisterCompany_ForOwnerCompanyWithAnotherExistedCompanyAndOwnerCompany_ShouldThrowOwnerCompanyAlreadyExistsException()
    {
        //arrange
        List<Company> companies = new List<Company>()
        {
            _company,
            _ownerCompany
        };
        
        //act
        var exception = Record.Exception(() => _companyRegistrationService.RegisterCompany(companies, Guid.NewGuid(), "newCompany", true,
            "new.pl"));
        
        //assert
        exception.Should().BeOfType<OwnerCompanyAlreadyExistsException>();
    }

    [Fact]
    public void RegisterCompany_ForCompanyWithTheSameName_ShouldThrowCompanyNameAlreadyExists()
    {
        //arrange
        var employeeId = Guid.NewGuid();
         _ownerCompany.AddEmployee(employeeId, "test@owner.pl");
         _ownerCompany.RegisterUser(_passwordPolicy, _passwordManagerMock.Object, 
             employeeId, Guid.NewGuid(), new FullName("test", "test"), "Pass123!");
        List<Company> companies = new List<Company>()
        {
            _company,
            _ownerCompany
        };
        
        //act
        var exception = Record.Exception(() => _companyRegistrationService.RegisterCompany(companies, Guid.NewGuid(), _company.Name, false,
            "new.pl", new TimeSpan(10000)));
        
        //assert
        exception.Should().BeOfType<CompanyNameAlreadyExistsException>();
    }
    
    [Fact]
    public void RegisterCompany_ForCompanyWithNotUniqueEmailDomain_ShouldThrowCompanyEmailDomainAlreadyExists()
    {
        //arrange
        var employeeId = Guid.NewGuid();
        _ownerCompany.AddEmployee(employeeId, "test@owner.pl");
        _ownerCompany.RegisterUser(_passwordPolicy, _passwordManagerMock.Object, 
            employeeId, Guid.NewGuid(), new FullName("test", "test"), "Pass123!");
        List<Company> companies = new List<Company>()
        {
            _company,
            _ownerCompany
        };
        
        //act
        var exception = Record.Exception(() => _companyRegistrationService.RegisterCompany(companies, Guid.NewGuid(), "newCompanyName", false,
            _company.EmailDomain, new TimeSpan(10000)));
        
        //assert
        exception.Should().BeOfType<CompanyEmailDomainAlreadyExists>();
    }
    
    #region arrange

    private readonly IPasswordPolicy _passwordPolicy;
    private readonly Mock<IPasswordManager> _passwordManagerMock;
    private readonly Company _company;
    private readonly Company _ownerCompany;
    private readonly ICompanyRegistrationService _companyRegistrationService;
    
    public CompanyRegistrationServiceTests()
    {
        _passwordPolicy = new UserPasswordPolicy();
        _passwordManagerMock = new Mock<IPasswordManager>();
        _passwordManagerMock
            .Setup(f => f.Secure(It.IsAny<string>()))
            .Returns("StrongPass123!");
        _company = Company.CreateCompany(Guid.NewGuid(), "testCompanyName", 
            new TimeSpan(10,10,10), "test.pl");
        _ownerCompany = Company.CreateOwnerCompany(Guid.NewGuid(), "testOwnerCompanyName",
            "owner.pl");
        _companyRegistrationService = new CompanyRegistrationService();
    }
    

    #endregion
}