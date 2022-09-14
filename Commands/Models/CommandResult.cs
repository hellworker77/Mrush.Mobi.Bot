namespace Commands.Models;

public class CommandResult
{
    public bool IsSuccessful { get; set; }
    public bool IsTerminating { get; set; }
    public string Message { get; set; }
}