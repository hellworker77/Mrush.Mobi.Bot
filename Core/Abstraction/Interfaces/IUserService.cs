using Domain.Models;

namespace Core.Abstraction.Interfaces;

public interface IUserService
{
    public Task AddUserAsync(User user);
    public Task DeleteUserAsync(long telegramId);
    public Task AddAccountAsync(long telegramId, Account account);
    public Task<User> GetByTelegramIdAsync(long telegramId);

}