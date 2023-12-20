using working_good.business.core.Abstractions.Repositories;
using working_good.business.core.Models;

namespace working_good.business.infrastructure.DAL.Repositories;

internal sealed class InMemoryUserRepository : IUserRepository
{
    private static List<User> _users = new List<User>();

    public Task<User> GetByEmailAsync(string email)
        => Task.FromResult(_users.FirstOrDefault(x => x.Email == email));

    public Task<User> GetByVerificationToken(string verificationToken)
        => Task.FromResult(_users.FirstOrDefault(x => x.VerificationToken.Token == verificationToken));

    public Task AddAsync(User user)
    {
        _users.Add(user);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(User user)
        => Task.CompletedTask;
}