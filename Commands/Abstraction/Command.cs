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
    }

    protected abstract string CommandString { get; }
    protected abstract IEnumerable<string>? CommandArgs { get; set; }
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
    public async Task<(bool wasSuccessful, bool shouldQuit)> RunCommand()
    {
        return (await InternalCommand(), IsTerminatingCommand);
    }

    protected abstract Task<bool> InternalCommand();

    public static Func<IServiceProvider, Func<string, Command>> GetCommand =>
        provider => input =>
        {
            var command = provider.GetServices<Command>().First(c => c.IsCommandFor(input));

            return command;
        };
}