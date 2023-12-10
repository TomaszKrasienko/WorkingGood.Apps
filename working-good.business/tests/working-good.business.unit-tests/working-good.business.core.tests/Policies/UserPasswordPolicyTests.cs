using FluentAssertions;
using working_good.business.core.Policies.Abstractions;
using working_good.business.core.Policies;
using Xunit;

namespace working_good.business.core.tests.Policies;

public class UserPasswordPolicyTests
{
    [Theory]
    [InlineData("Test")]
    [InlineData("test123")]
    [InlineData("TEST123")]
    [InlineData("testTEST")]
    public void Secure_ForInvalidPassword_ShouldReturnFalse(string password)
    {
        //Act
        var result = _passwordPolicy.VerifyPassword(password);
        //Assert
        result.Should().BeFalse();
    }

    [Theory]
    [InlineData("Test123")]
    public void Secure_ForValidPassword_ShouldReturnTrue(string password)
    {
        //Act
        var result = _passwordPolicy.VerifyPassword(password);
        //Assert
        result.Should().BeTrue();
    }
    
    #region arrange

    private readonly IPasswordPolicy _passwordPolicy;
    
    public UserPasswordPolicyTests()
    {
        _passwordPolicy = new UserPasswordPolicy();
    }
    #endregion
}