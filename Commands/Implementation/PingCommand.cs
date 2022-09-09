using Commands.Abstraction;
using Domain.Abstraction.Interfaces;
using Requests.Abstraction;

namespace Commands.Implementation;

public class PingCommand : Command
{
    private readonly Func<string, Request> _requestFactory;
    private readonly IBrowser _browser;
    public PingCommand(IShowMessage showMessage,
        Func<string, Request> requestFactory,
        IBrowser browser) : base(showMessage)
    {
        _requestFactory = requestFactory;
        _browser = browser;
    }

    protected override string CommandString => "ping";
    protected override async Task<bool> InternalCommand()
    {
        var request = _requestFactory("ping");

        var requestResult = await request.SendRequestAsync();

        if (requestResult)
        {
            ShowMessage.ShowInfo($"Your are authorized");
        }
        else
        {
            ShowMessage.ShowInfo($"Your are not authorized");
        }

        return true;
    }
}