using System.Linq.Expressions;
using Commands.Abstraction;
using Commands.Models;
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
    protected override async Task<CommandResult> InternalCommandExecute()
    {
        long telegramId;

        var result = new CommandResult
        {
            IsSuccessful = false,
            Message = "Account didn't create"
        };

        var resultParsing = long.TryParse(CommandArgs.ElementAt(0), out telegramId);

        if (CommandArgs.Count() != 3 || !resultParsing)
        {
            return await Task.FromResult(result);
        }

        var account = new Account()
        {
            Login = CommandArgs.ElementAt(1),
            Password = CommandArgs.ElementAt(2)
        };
        result.IsSuccessful = true;
        result.Message = "Account created";

        await _userService.AddAccountAsync(telegramId, account);
        return await Task.FromResult(result);

    }
}