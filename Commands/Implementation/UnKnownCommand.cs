using Commands.Abstraction;
using Commands.Models;
using Domain.Abstraction.Interfaces;

namespace Commands.Implementation;

public class UnKnownCommand : Command
{
    public UnKnownCommand(IShowMessage showMessage) : base( showMessage)
    {
    }

    protected override string CommandString => string.Empty;
    protected override bool IsCommandFor(string input)
    {
        return true;
    }

    protected override async Task<CommandResult> InternalCommandExecute()
    {
        var result = new CommandResult
        {
            IsSuccessful = false,
            Message = "Unknow command"
        };
        return await Task.FromResult(result);
    }

   
}