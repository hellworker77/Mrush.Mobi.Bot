using Domain.Models;
using Entities.Implementation;

namespace Data.Abstraction.Interfaces;

public interface IUserRepository
{
    public Task<User> GetByTelegramIdAsync(long telegramId);
    public Task AddAsync(User user);
    public Task DeleteAsync(Guid id);
    public Task UpdateAsync(User user);

}