namespace Domain.Abstraction.Interfaces;

public interface ITimer
{
    public int Delay { get; set; }
    public Func<Task> TargetMethod { get; set; }
    public bool IsRunning { get; set; }

    public Task StartAsync();
    public Task StopAsync();
    public void UpdateDelay(int delay);
}