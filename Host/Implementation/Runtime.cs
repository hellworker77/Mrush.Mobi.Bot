using Commands.Abstraction;
using Domain.Abstraction.Interfaces;
using Host.Abstraction.Interfaces;

namespace Host.Implementation;

public class Runtime : IRuntime
{
    private bool _isRunning;
    private readonly IShowMessage _showMessage;
    private readonly ITimer _timer;
    private readonly Func<string, Command> _commandFactory;

    public Runtime(IShowMessage showMessage,
        ITimer timer,
        Func<string, Command> commandFactory)
    {
        IsRunning = true;
        _showMessage = showMessage;
        _timer = timer;
        _commandFactory = commandFactory;
    }



    public async Task RunAsync()
    {

    }
    public async Task ApplyCommand(string command)
    {
        await RunCommandAsync(command);
    }
    public async Task InitializeAsync()
    {
        _timer.UpdateDelay(5000);

        var taskReadCommandFromConsole = ReadCommandFromConsoleAsync();
        _timer.TargetMethod = RunAsync;

        var runtime = Task.WhenAll(_timer.StartAsync(), taskReadCommandFromConsole);
        await runtime;
    }

    private async Task ReadCommandFromConsoleAsync()
    {
        await Task.Delay(10);
        while (IsRunning)
        {
            var nextCommand = Console.ReadLine() ?? string.Empty;

            var consolePointerTopPosition = Console.GetCursorPosition().Top;
            Console.SetCursorPosition(0, consolePointerTopPosition - 1);

            _showMessage.ShowAsUser(nextCommand);

            await ApplyCommand(nextCommand);

        }
    }
    private async Task RunCommandAsync(string currentCommand)
    {
        var separator = '|';
        var command = _commandFactory(currentCommand);

        command.ImportCommandArgs(currentCommand, separator);

        var response = await command.RunCommand();

        IsRunning = !response.shouldQuit;
    }
    public bool IsRunning
    {
        get => _isRunning;
        private set => _isRunning = value;
    }

}