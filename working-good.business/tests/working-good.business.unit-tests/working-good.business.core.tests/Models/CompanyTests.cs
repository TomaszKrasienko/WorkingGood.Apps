using FluentAssertions;
using Moq;
using working_good.business.core.Abstractions;
using working_good.business.core.DomainServices;
using working_good.business.core.DomainServices.Abstractions;
using working_good.business.core.Events;
using working_good.business.core.Exceptions;
using working_good.business.core.Models;
using working_good.business.core.Models.Company;
using working_good.business.core.Policies;
using working_good.business.core.Policies.Abstractions;
using working_good.business.core.ValueObjects.User;
using Xunit;

namespace working_good.business.core.tests.Models;

public sealed class CompanyTests
{
    [Fact]
    public void AddEmployee_ForValidEmailWithValidDomain_ShouldAddEmployeeToCompany()
    {
        //arrange
        var employeeId = Guid.NewGuid();
        var employeeEmail = "test@test.pl";
        
        //act
        _company.AddEmployee(employeeId, employeeEmail);
        
        //Assert
        _company.Employees.Any(x
            => x.Id == employeeId
               && x.Email == employeeEmail).Should().BeTrue();
    }

    [Fact]
    public void AddEmployee_ForAlreadyExistedEmployee_ShouldThrowEmailAlreadyExistsException()
    {
        //arrange
        var employeeId = Guid.NewGuid();
        var employeeEmail = "test@test.pl";
        _company.AddEmployee(Guid.NewGuid(), employeeEmail);
        
        //act
        var exception = Record.Exception(() => _company.AddEmployee(employeeId, employeeEmail));
        
        //assert
        exception.Should().BeOfType<EmailAlreadyInUseException>();
    }

    [Fact]
    public void AddEmployee_ForNotMatchingEmailDomain_ShouldThrowNotMatchingEmployeeEmailDomainException()
    {
        //arrange
        var employeeId = Guid.NewGuid();
        var employeeEmail = $"test@invalid{_company.EmailDomain.Value}";
        
        //act
        var exception = Record.Exception(() => _company.AddEmployee(employeeId, employeeEmail));
        
        //assert
        exception.Should().BeOfType<NotMatchingEmployeeEmailDomainException>();
    }

     [Fact]
     public void VerifyUser_ForValidToken_ShouldActivateUser()
     {
         //arrange
         var company = Company.CreateCompany(Guid.NewGuid(), "testCompanyName", 
             new TimeSpan(00, 00, 10), "test.pl");
         var employeeId = Guid.NewGuid();
         company.AddEmployee(employeeId, "test@test.pl");
         _passwordManagerMock
             .Setup(f => f.Secure(It.IsAny<string>())).Returns("securedPassword");
         company.RegisterUser(_passwordPolicy, _passwordManagerMock.Object, employeeId,Guid.NewGuid(), 
             new FullName("firstName", "lastName"), "strongPass123!", Role.User());
         var user = company.Employees.First(x => x.Id == employeeId).User;
         
         //act
         company.VerifyUser(user.VerificationToken.Token);
         
         //assert
         user.VerificationToken.VerificationDate.Should().NotBeNull();
     }

     [Fact]
     public void CanUserBeLogged_ForActiveAndExistingUser_ShouldReturnTrue()
     {
         //arrange
         string email = "test@test.pl";
         var company = Company.CreateCompany(Guid.NewGuid(), "testCompanyName", 
             new TimeSpan(00, 00, 10), "test.pl");
         var employeeId = Guid.NewGuid();
         company.AddEmployee(employeeId, email);
         _passwordManagerMock
             .Setup(f => f.Secure(It.IsAny<string>())).Returns("securedPassword");
         company.RegisterUser(_passwordPolicy, _passwordManagerMock.Object, employeeId,Guid.NewGuid(), 
             new FullName("firstName", "lastName"), "strongPass123!", Role.User());
         var user = company.Employees.First(x => x.Id == employeeId).User;
         company.VerifyUser(user.VerificationToken.Token);
         
         //act
         var result = company.CanUserBeLogged(email);
         
         //Assert
         result.Should().BeTrue();
     }
     
     [Fact]
     public void CanUserBeLogged_ForNoActiveUser_ShouldReturnFalse()
     {
         //arrange
         string email = "test@test.pl";
         var company = Company.CreateCompany(Guid.NewGuid(), "testCompanyName", 
             new TimeSpan(00, 00, 10), "test.pl");
         var employeeId = Guid.NewGuid();
         company.AddEmployee(employeeId, email);
         _passwordManagerMock
             .Setup(f => f.Secure(It.IsAny<string>())).Returns("securedPassword");
         company.RegisterUser(_passwordPolicy, _passwordManagerMock.Object, employeeId,Guid.NewGuid(), 
             new FullName("firstName", "lastName"), "strongPass123!", Role.User());
         var user = company.Employees.First(x => x.Id == employeeId).User;
         
         //act
         var result = company.CanUserBeLogged(email);
         
         //Assert
         result.Should().BeFalse();
     }
     
     [Fact]
     public void CanUserBeLogged_ForNotExistingUser_ShouldReturnFalse()
     {
         //arrange
         string email = "test@test.pl";
         var company = Company.CreateCompany(Guid.NewGuid(), "testCompanyName", 
             new TimeSpan(00, 00, 10), "test.pl");
         var employeeId = Guid.NewGuid();
         company.AddEmployee(employeeId, email);
         _passwordManagerMock
             .Setup(f => f.Secure(It.IsAny<string>())).Returns("securedPassword");
         
         //act
         var result = company.CanUserBeLogged(email);
         
         //Assert
         result.Should().BeFalse();
     }
     
     #region arrange
     private readonly Company _company;
     private readonly IPasswordPolicy _passwordPolicy;
     private readonly Mock<IPasswordManager> _passwordManagerMock;
     
     public CompanyTests()
     {        
         _company = Company.CreateCompany(Guid.NewGuid(), "testCompanyName", 
             new TimeSpan(10,10,10), "test.pl");
         _passwordPolicy = new UserPasswordPolicy();
         _passwordManagerMock = new Mock<IPasswordManager>();
     }
     #endregion
}