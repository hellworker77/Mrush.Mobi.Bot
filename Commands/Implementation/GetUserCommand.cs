using Commands.Abstraction;
using Commands.Models;
using Core.Abstraction.Interfaces;
using Domain.Abstraction.Interfaces;
using Domain.Models;

namespace Commands.Implementation;

public class GetUserCommand : Command
{
    private readonly IUserService _userService;
    public GetUserCommand(IShowMessage showMessage,
        IUserService userService) : base(showMessage)
    {
        _userService = userService;
    }

    protected override string CommandString => "get-user";

    protected override bool IsCommandFor(string input)
    {
        return input.Contains(CommandString);
    }

    protected override async Task<CommandResult> InternalCommandExecute()
    {
        var result = new CommandResult
        {
            IsSuccessful = false,
            Message = "Not found user"
        };
        if (CommandArgs.Count() != 1)
        {
            return await Task.FromResult(result);
        }

        var telegramId = long.Parse(CommandArgs.ElementAt(0));

        var user = await _userService.GetByTelegramIdAsync(telegramId);

        result.IsSuccessful = true;
        result.Message = $"{user?.ToString() ?? string.Empty}";

        return await Task.FromResult(result);
    }
}