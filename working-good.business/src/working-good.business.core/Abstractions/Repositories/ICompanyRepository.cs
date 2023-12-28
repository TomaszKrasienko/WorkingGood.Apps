using working_good.business.core.Models.Company;

namespace working_good.business.core.Abstractions.Repositories;

public interface ICompanyRepository
{
    Task<List<Company>> GetAllAsync();
    Task<Company> GetByIdAsync(Guid companyId);
    Task<Company> GetByUserEmailAsync(string email);
    Task<Company> GetByUserVerificationTokenAsync(string verificationToken);
    Task AddAsync(Company company);
    Task UpdateAsync(Company company);
}