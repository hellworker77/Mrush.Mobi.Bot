using Commands.Abstraction;
using Domain.Abstraction.Interfaces;
using Requests.Abstraction;

namespace Commands.Implementation;

public class PingCommand : Command
{
    private readonly Func<string, Request> _requestFactory;
    public PingCommand(IShowMessage showMessage,
        Func<string, Request> requestFactory) : base(showMessage)
    {
        _requestFactory = requestFactory;
    }

    protected override string CommandString => "/ping";
    protected override IEnumerable<string>? CommandArgs { get; set; }
    protected override async Task<bool> InternalCommand()
    {
        var request = _requestFactory("ping");

        var statusCode = await request.SendRequest();

        

        ShowMessage.ShowInfo($"pinged with code {statusCode}");

        return true;
    }
}