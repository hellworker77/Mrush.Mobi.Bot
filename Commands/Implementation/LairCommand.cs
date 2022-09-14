using Commands.Abstraction;
using Commands.Models;
using Domain.Abstraction.Interfaces;
using Requests.Abstraction;

namespace Commands.Implementation;

public class LairCommand: Command
{
    private Func<string, Request> _requestFactory;
    public LairCommand(IShowMessage showMessage,
    Func<string, Request> requestFactory) : base(showMessage)
    {
        _requestFactory = requestFactory;
    }

    protected override string CommandString => "lair";
    protected override bool IsCommandFor(string input)
    {
        return CommandString == input;
    }

    protected override async Task<CommandResult> InternalCommandExecute()
    {
        do
        {
            var request = _requestFactory("lair");
            var requestResult = await request.SendRequestAsync();
            if (requestResult == "Посмотреть" || requestResult == "Обновить" || requestResult ==  "")
            {
                break;
            }
            await Task.Delay(300);
        } while (true);

        var result = new CommandResult
        {
            IsSuccessful = true,
            Message = "Lair completed"
        };

        return await Task.FromResult(result);
    }
}