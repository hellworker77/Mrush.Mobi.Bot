using AutoMapper;
using Core.Abstraction.Interfaces;
using Data.Abstraction.Interfaces;
using Domain.Models;
using Entities.Implementation;

namespace Core.Implementation;

public class UserService : IUserService
{
    private readonly IAccountRepository _accountRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    public UserService(IAccountRepository accountRepository,
        IUserRepository userRepository,
        IMapper mapper)
    {
        _accountRepository = accountRepository;
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task AddUserAsync(User user)
    {
        await _userRepository.AddAsync(user);
    }

    public async Task DeleteUserAsync(long telegramId)
    {
        var user = await _userRepository.GetByTelegramIdAsync(telegramId);
        await _userRepository.DeleteAsync(user.Id);
    }

    public async Task AddAccountAsync(long telegramId, Account account)
    {
        var user = await _userRepository.GetByTelegramIdAsync(telegramId);

        account.UserId = user.Id;
        await _accountRepository.AddAccountAsync(account);


        user.Accounts.Add(account);
        await _userRepository.UpdateAsync(user);
    }

    public async Task<User> GetByTelegramIdAsync(long telegramId)
    {
        return await _userRepository.GetByTelegramIdAsync(telegramId);
    }
}