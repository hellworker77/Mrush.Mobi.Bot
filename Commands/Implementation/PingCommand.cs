using Commands.Abstraction;
using Commands.Models;
using Domain.Abstraction.Interfaces;
using Newtonsoft.Json;
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

    protected override string CommandString => "ping";
    protected override async Task<CommandResult> InternalCommandExecute()
    {
        var request = _requestFactory("ping");

        var requestSerializedResult = await request.SendRequestAsync();
        var deserialized = JsonConvert.DeserializeObject<bool>(requestSerializedResult);

        var result = new CommandResult
        {
            IsSuccessful = deserialized,
            Message = deserialized ? "You are authorized" : "You are not authorized"
        };

        return result;
    }
}