using Commands.Abstraction;
using Commands.Models;
using Domain.Abstraction.Interfaces;

namespace Commands.Implementation;

public class ShutdownCommand : Command
{
    private ITimer _timer;
    public ShutdownCommand(IShowMessage showMessage,
        ITimer timer) : base(showMessage)
    {
        _timer = timer;
        IsTerminatingCommand = true;
    }


    protected override string CommandString => "shutdown";
    protected override async Task<CommandResult> InternalCommandExecute()
    {
        await _timer.StopAsync();
        var result = new CommandResult
        {
            IsSuccessful = true,
            Message = "Server stopped"
        };

        return await Task.FromResult(result);
    }
}