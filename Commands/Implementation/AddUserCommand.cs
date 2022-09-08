using Commands.Abstraction;
using Core.Abstraction.Interfaces;
using Domain.Abstraction.Interfaces;
using Domain.Models;

namespace Commands.Implementation;

public class AddUserCommand : Command
{
    private readonly IUserService _userService;
    public AddUserCommand(IShowMessage showMessage,
        IUserService userService) : base(showMessage)
    {
        _userService = userService;
    }

    protected override string CommandString => "/register";

    protected override bool IsCommandFor(string input)
    {
        return CommandString == input;
    }

    protected override async Task<bool> InternalCommand()
    {
        var telegramId = 1867699456;

        var user = new User
        {
            TelegramId = telegramId
        };

        await _userService.AddUserAsync(user);

        var account = new Account
        {
            Login = "секси tt",
            Password = "anarms1890"
        };

        await _userService.AddAccountAsync(telegramId, account);

        var dto = await _userService.GetByTelegramIdAsync(telegramId);

        ShowMessage.ShowAsConsole("Save user successful");


        return await Task.FromResult(true);
    }
}