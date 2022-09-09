using AutoMapper;
using Core.Abstraction.Interfaces;
using Data.Abstraction.Interfaces;
using Domain.Abstraction.Interfaces;
using Domain.Models;
using Entities.Implementation;

namespace Core.Implementation;

public class UserService : IUserService
{
    private readonly IAccountRepository _accountRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IShowMessage _showMessage;
    public UserService(IAccountRepository accountRepository,
        IUserRepository userRepository,
        IMapper mapper,
        IShowMessage showMessage)
    {
        _accountRepository = accountRepository;
        _userRepository = userRepository;
        _mapper = mapper;
        _showMessage = showMessage;
    }

    public async Task AddUserAsync(User user)
    {
        await _userRepository.AddAsync(user);
        _showMessage.ShowSuccessful($"Added new user with telegramId : {user.TelegramId}");
    }

    public async Task DeleteUserAsync(long telegramId)
    {
        var user = await _userRepository.GetByTelegramIdAsync(telegramId);
        if (user == null)
        {
            _showMessage.ShowError($"Not found user with telegramId : {telegramId}");
            return;
        }
        await _userRepository.DeleteAsync(user.Id);
        _showMessage.ShowSuccessful($"Deleted user with Id : {telegramId}");
    }

    public async Task AddAccountAsync(long telegramId, Account account)
    {
        var user = await _userRepository.GetByTelegramIdAsync(telegramId);

        if (user == null)
        {
            _showMessage.ShowError($"Not found user with telegramId : {telegramId}");
            return;
        }

        account.UserId = user.Id;
        await _accountRepository.AddAccountAsync(account);


        user.Accounts.Add(account);
        await _userRepository.UpdateAsync(user);
        _showMessage.ShowSuccessful($"Added new account to : {telegramId}");
    }

    public async Task<User> GetByTelegramIdAsync(long telegramId)
    {
        return await _userRepository.GetByTelegramIdAsync(telegramId);
    }
}