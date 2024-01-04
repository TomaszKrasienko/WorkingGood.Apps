using working_good.business.core.Abstractions.Repositories;
using working_good.business.core.Models.Company;
using Microsoft.EntityFrameworkCore;

namespace working_good.business.infrastructure.DAL.Repositories;

internal sealed class PostgreCompanyRepository(WgDbContext context) : ICompanyRepository
{

    public Task<List<Company>> GetAllAsync()
        => context
            .Companies
            .Include(x => x.Employees)
            .ThenInclude(x => x.User)
            .ToListAsync();

    public Task<Company> GetByIdAsync(Guid companyId)
        => context
            .Companies
            .Include(x => x.Employees)
            .ThenInclude(x => x.User)
            .FirstOrDefaultAsync(x => x.Id == companyId);

    public Task<Company> GetByEmployeeEmailAsync(string email)
        => context
            .Companies
            .Include(x => x.Employees)
            .ThenInclude(x => x.User)
            .FirstOrDefaultAsync(x => x.Employees.Any(x => x.Email == email));

    public Task<Company> GetByUserVerificationTokenAsync(string verificationToken)
        => context
            .Companies
            .Include(x => x.Employees)
            .ThenInclude(x => x.User)
            .FirstOrDefaultAsync(x
                => x.Employees.Any(y => y.User.VerificationToken.Token == verificationToken));

    public async Task AddAsync(Company company)
    {
        await context.Companies.AddAsync(company);
        await context.SaveChangesAsync();
    }


    public Task UpdateAsync(Company company)
        => context.SaveChangesAsync();
}