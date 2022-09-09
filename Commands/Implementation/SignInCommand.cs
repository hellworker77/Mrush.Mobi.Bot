using Commands.Abstraction;
using Domain.Abstraction.Interfaces;
using Requests.Abstraction;

namespace Commands.Implementation;

public class SignInCommand : Command
{
    private readonly Func<string, Request> _requestFactory;
    private readonly IBrowser _browser;
    public SignInCommand(IShowMessage showMessage,
        Func<string, Request> requestFactory,
        IBrowser browser) : base(showMessage)
    {
        _requestFactory = requestFactory;
        _browser = browser;
    }
    protected override string CommandString => "signIn";
    protected override bool IsCommandFor(string input)
    {
        return input.Contains(CommandString);
    }

    protected override async Task<bool> InternalCommand()
    {
        var request = _requestFactory("signIn");

        if (CommandArgs.Count() != 2)
        {
            ShowMessage.ShowAsConsole("Not valid command");
            return false;
        }
        var name = CommandArgs.ElementAt(0);
        var password = CommandArgs.ElementAt(1);

        request.ImportRequestArgs($"{name}|{password}", '|');

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