using Commands.Abstraction;
using Domain.Abstraction.Interfaces;

namespace Commands.Implementation;

public class UnKnownCommand : Command
{
    public UnKnownCommand(IShowMessage showMessage) : base( showMessage)
    {
    }

    protected override string CommandString => string.Empty;
    protected override IEnumerable<string>? CommandArgs { get; set; }
    protected override bool IsCommandFor(string input)
    {
        return true;
    }

    protected override async Task<bool> InternalCommand()
    {
       ShowMessage.ShowAsConsole("Not found command");
       return await Task.FromResult(true);
    }

   
}