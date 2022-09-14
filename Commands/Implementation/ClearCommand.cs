using Commands.Abstraction;
using Commands.Models;
using Domain.Abstraction.Interfaces;

namespace Commands.Implementation;

public class ClearCommand : Command
{
    public ClearCommand(IShowMessage showMessage) : base(showMessage)
    {
    }

    protected override string CommandString => "cls";
    protected override bool IsCommandFor(string input)
    {
        return CommandString == input;
    }

    protected override Task<CommandResult> InternalCommandExecute()
    {
        var result = new CommandResult
        {
            IsSuccessful = true,
            Message = ""
        };
        Console.Clear();
        return Task.FromResult(result);
    }
}