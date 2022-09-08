using Domain.Abstraction.Interfaces;

namespace Domain.Implementation;

public class Timer : ITimer
{
    public int Delay { get; set; }
    public Func<Task> TargetMethod { get; set; }

    public bool IsRunning { get; set; }
    private async Task RunAsync()
    {
        while (IsRunning)
        {
            await Task.Delay(Delay);
            if (TargetMethod != null)
            {
                await TargetMethod?.Invoke();
            }


        }

    }

    public void UpdateDelay(int delay)
    {
        Delay = delay;
    }

    public async Task StartAsync()
    {
        IsRunning = true;
        await RunAsync();
    }

    public async Task StopAsync()
    {
        IsRunning = false;
    }
}