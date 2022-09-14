using Commands.Abstraction;
using Commands.Models;
using Domain.Abstraction.Interfaces;
using Newtonsoft.Json;
using Requests.Abstraction;

namespace Commands.Implementation;

public class LogoutCommand : Command
{
    private readonly Func<string, Request> _requestFactory;
    public LogoutCommand(IShowMessage showMessage,
        Func<string, Request> requestFactory) : base(showMessage)
    {
        _requestFactory = requestFactory;
    }

    protected override string CommandString => "logout";
    protected override async Task<CommandResult> InternalCommandExecute()
    {
        var request = _requestFactory("logout");

        var requestSerializedResult = await request.SendRequestAsync();
        var deserialized = !JsonConvert.DeserializeObject<bool>(requestSerializedResult);

        var result = new CommandResult
        {
            IsSuccessful = deserialized,
            Message = deserialized? "You are logout": "You are not logout"
        };

        return await Task.FromResult(result);
    }
}