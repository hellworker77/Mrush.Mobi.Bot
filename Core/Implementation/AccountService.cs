using Core.Abstraction.Interfaces;
using Data.Abstraction.Interfaces;
using Domain.Models;
using Entities.Implementation;

namespace Core.Implementation;

public class AccountService : IAccountService
{
    private readonly IAccountRepository _accountRepository;
    public AccountService(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }
    public async Task<Account> GetAccountByIdAsync(Guid accountId)
    {
        return await _accountRepository.GetAccountByIdAsync(accountId);
    }

    public async Task UpdateAccountAsync(Account account)
    {
        await _accountRepository.UpdateAccountAsync(account);
    }

    public async Task DeleteAccountAsync(Guid accountId)
    {
        await _accountRepository.DeleteAccountAsync(accountId);
    }
}