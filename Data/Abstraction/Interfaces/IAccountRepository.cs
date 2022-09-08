using Domain.Models;
using Entities.Implementation;

namespace Data.Abstraction.Interfaces;

public interface IAccountRepository
{
    public Task<Account> GetAccountByIdAsync(Guid accountId);
    public Task AddAccountAsync(Account account);
    public Task UpdateAccountAsync(Account account);
    public Task DeleteAccountAsync(Guid accountId);
}