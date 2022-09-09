using Commands.Abstraction;
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
    protected override async Task<bool> InternalCommand()
    {
        ShowMessage.ShowInfo("shutdown...");
        await _timer.StopAsync();
        ShowMessage.ShowInfo("stopped");

        return await Task.FromResult(true);
    }
}