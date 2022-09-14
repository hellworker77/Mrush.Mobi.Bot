using Commands.Abstraction;
using Commands.Models;
using Domain.Abstraction.Interfaces;
using Newtonsoft.Json;
using Requests.Abstraction;

namespace Commands.Implementation;

public class SignInCommand : Command
{
    private readonly Func<string, Request> _requestFactory;
    public SignInCommand(IShowMessage showMessage,
        Func<string, Request> requestFactory) : base(showMessage)
    {
        _requestFactory = requestFactory;
    }
    protected override string CommandString => "signIn";
    protected override bool IsCommandFor(string input)
    {
        return input.Contains(CommandString);
    }

    protected override async Task<CommandResult> InternalCommandExecute()
    {
        var request = _requestFactory("signIn");
        var result = new CommandResult
        {
            IsSuccessful = false,
            Message = "Not valid command"
        };

        if (CommandArgs.Count() != 2)
        {
            return result;
        }
        var name = CommandArgs.ElementAt(0);
        var password = CommandArgs.ElementAt(1);

        request.ImportRequestArgs($"{name}|{password}", '|');

        var requestSerializedResult = await request.SendRequestAsync();
        var deserialized = JsonConvert.DeserializeObject<bool>(requestSerializedResult);

        result.IsSuccessful = deserialized;
        result.Message = deserialized ? "You are authorized" : "You sent wrong login or password";

        return result;
    }
}