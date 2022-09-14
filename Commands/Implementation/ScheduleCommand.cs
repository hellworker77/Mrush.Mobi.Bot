using Commands.Abstraction;
using Commands.Models;
using Domain.Abstraction.Interfaces;
using Requests.Abstraction;

namespace Commands.Implementation;

public class ScheduleCommand : Command
{
    private readonly Func<string, Request> _requestFactory;
    public ScheduleCommand(IShowMessage showMessage,
        Func<string, Request> requestFactory) : base(showMessage)
    {
        _requestFactory = requestFactory;
    }

    protected override string CommandString => "get-schedule";
    protected override async Task<CommandResult> InternalCommandExecute()
    {
        var request = _requestFactory("schedule");

        var requestResult = await request.SendRequestAsync();

        var result = new CommandResult
        {
            IsSuccessful = requestResult!=string.Empty,
            Message = requestResult
        };

        return result;
    }
}