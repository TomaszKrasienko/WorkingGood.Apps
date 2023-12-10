using working_good.business.core.Models;

namespace working_good.business.core.Abstractions.Repositories;

public interface IUserRepository
{
    Task AddAsync(User user);
}