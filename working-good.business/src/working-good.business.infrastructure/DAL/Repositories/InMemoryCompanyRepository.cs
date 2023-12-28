using working_good.business.core.Abstractions.Repositories;
using working_good.business.core.Models.Company;

namespace working_good.business.infrastructure.DAL.Repositories;

internal sealed class InMemoryCompanyRepository : ICompanyRepository
{
    private static List<Company> _companies = new List<Company>();

    public Task<List<Company>> GetAllAsync()
        => Task.FromResult(_companies);

    public Task<Company> GetByIdAsync(Guid companyId)
        => Task.FromResult(_companies.FirstOrDefault(x => x.Id == companyId));

    public Task<Company> GetByUserEmailAsync(string email)
        => Task.FromResult(_companies.FirstOrDefault(x
            => x.Users.Any(arg => arg.Email == email)));
    

    public Task<Company> GetByUserVerificationTokenAsync(string verificationToken)
        => Task.FromResult(_companies.FirstOrDefault(x
            => x.Users.Any(arg => arg.VerificationToken.Token == verificationToken)));

    public Task AddAsync(Company company)
    {
        _companies.Add(company);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Company company)
        => Task.CompletedTask;
}