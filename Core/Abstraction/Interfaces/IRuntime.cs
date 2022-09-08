namespace Core.Abstraction.Interfaces;

public interface IRuntime
{
    public bool IsRunning { get; }
    public Task RunAsync();
    public Task InitializeAsync();
    public Task ApplyCommand(string command);

}