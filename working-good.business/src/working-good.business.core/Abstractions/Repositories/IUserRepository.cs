using working_good.business.core.Models;

namespace working_good.business.core.Abstractions.Repositories;

public interface IUserRepository
{
    Task<User> GetByEmailAsync(string email);
    Task<User> GetByVerificationToken(string verificationToken);
    Task AddAsync(User user);
    Task UpdateAsync(User user);
}