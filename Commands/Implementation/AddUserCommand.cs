using Commands.Abstraction;
using Commands.Models;
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

    protected override string CommandString => "reg";

    protected override bool IsCommandFor(string input)
    {
        return input.Contains(CommandString);
    }

    protected override async Task<CommandResult> InternalCommandExecute()
    {
        var result = new CommandResult
        {
            IsSuccessful = false,
            Message = "User didn't sign in"
        };
        long telegramId;

        var resultParsing = long.TryParse(CommandArgs.ElementAt(0), out telegramId);

        if (CommandArgs.Count() != 1 || !resultParsing)
        {
            return await Task.FromResult(result);
        }
        
        var user = new User
        {
            TelegramId = telegramId
        };

        await _userService.AddUserAsync(user);


        result.IsSuccessful = true;
        result.Message = "User signed in";


        return await Task.FromResult(result);
    }
}