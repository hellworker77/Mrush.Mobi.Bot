using System.Linq.Expressions;
using Commands.Abstraction;
using Core.Abstraction.Interfaces;
using Domain.Abstraction.Interfaces;
using Domain.Models;

namespace Commands.Implementation;

public class AddAccountCommand : Command
{
    private readonly IUserService _userService;
    public AddAccountCommand(IShowMessage showMessage,
        IUserService userService) : base(showMessage)
    {
        _userService = userService;
    }

    protected override string CommandString => "add-account";
    protected override bool IsCommandFor(string input)
    {
        return input.Contains(CommandString);
    }
    protected override async Task<bool> InternalCommand()
    {
        long telegramId = 0;

        var resultParsing = long.TryParse(CommandArgs.ElementAt(0), out telegramId);

        if (CommandArgs.Count() != 3 || !resultParsing)
        {
            ShowMessage.ShowAsConsole("Not valid command");
            return await Task.FromResult(false);
        }

        var account = new Account()
        {
            Login = CommandArgs.ElementAt(1),
            Password = CommandArgs.ElementAt(2)
        };
        

        await _userService.AddAccountAsync(telegramId, account);
        return await Task.FromResult(true);

    }
}