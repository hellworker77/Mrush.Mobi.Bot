using Commands.Models;
using Domain.Abstraction.Interfaces;
using Microsoft.Extensions.DependencyInjection;
namespace Commands.Abstraction;

public abstract class Command
{
    protected bool IsTerminatingCommand;
    protected readonly IShowMessage ShowMessage;

    protected Command(IShowMessage showMessage)
    {
        IsTerminatingCommand = false;
        ShowMessage = showMessage;
        CommandArgs = new List<string>();
    }

    protected abstract string CommandString { get; }
    protected IEnumerable<string> CommandArgs { get; set; }
    protected virtual bool IsCommandFor(string input)
    {
        return CommandString.Contains(input.ToLower());
    }

    public virtual void ImportCommandArgs(string command, char separator)
    {
        var separatedCommand = command.Split(separator).ToList();
        var args = separatedCommand.Skip(1);

        CommandArgs = args;
    }

    public async Task<CommandResult> RunCommandAsync()
    {
        var result = await InternalCommandExecute();
        result.IsTerminating = IsTerminatingCommand;
        return result;
    }

    protected abstract Task<CommandResult> InternalCommandExecute();

    public static Func<IServiceProvider, Func<string, Command>> GetCommand =>
        provider => input =>
        {
            var command = provider.GetServices<Command>().First(c => c.IsCommandFor(input));

            return command;
        };
}