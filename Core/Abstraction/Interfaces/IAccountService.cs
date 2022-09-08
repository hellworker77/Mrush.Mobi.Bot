using Domain.Models;
using Entities.Implementation;

namespace Core.Abstraction.Interfaces;

public interface IAccountService
{
    public Task<Account> GetAccountByIdAsync(Guid accountId);
    public Task UpdateAccountAsync(Account account);
    public Task DeleteAccountAsync(Guid accountId);
}