using FluentAssertions;
using working_good.business.core.DomainServices;
using working_good.business.core.DomainServices.Abstractions;
using working_good.business.core.Models.Company;
using Xunit;

namespace working_good.business.core.tests.DomainServices;

public sealed class CompanyRegistrationServiceTests
{
    [Fact]
    public void RegisterCompany_ForCompanyWithAnotherExistedCompanyAndOwnerCompany_ShouldReturnCompany()
    {
        //arrange
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
    
    #region arrange

    private readonly Company _company;
    private readonly Company _ownerCompany;
    private readonly ICompanyRegistrationService _companyRegistrationService;
    
    public CompanyRegistrationServiceTests()
    {
        _company = Company.CreateCompany(Guid.NewGuid(), "testCompanyName", 
            new TimeSpan(10,10,10), "test.pl");
        _ownerCompany = Company.CreateOwnerCompany(Guid.NewGuid(), "testOwnerCompanyName",
            "owner.pl");
        _companyRegistrationService = new CompanyRegistrationService();
    }

    #endregion
}