using Domain.Abstraction.Interfaces;

namespace Domain.Implementation;

public class ShowMessage : IShowMessage
{
    private ConsoleColor defaultForeGroundColor;
    public ShowMessage()
    {
        defaultForeGroundColor = Console.ForegroundColor;
    }
    public void ShowError(string message)
    {
        Show(ConsoleColor.DarkYellow, ConsoleColor.DarkRed, "ERROR", message, "SERVER");
    }
    public void ShowWarning(string message)
    {
        Show(ConsoleColor.DarkYellow, ConsoleColor.Red, "WARNING", message, "SERVER");
    }
    public void ShowInfo(string message)
    {
        Show(ConsoleColor.DarkYellow, ConsoleColor.DarkCyan, "INFO", message, "SERVER");
    }
    public void ShowSuccessful(string message)
    {
        Show(ConsoleColor.DarkYellow, ConsoleColor.Green, "SUCCESS", message, "SERVER");
    }
    public void ShowAsUser(string message)
    {
        Show(ConsoleColor.DarkMagenta, ConsoleColor.White, "", message, "USER");
    }
    public void ShowAsConsole(string message)
    {
        Show(ConsoleColor.DarkBlue, ConsoleColor.White, "", message, "CONSOLE");
    }
    private void Show(ConsoleColor senderColor, ConsoleColor handlingMessageColor, string handlingMessage, string message, string sender)
    {
        var consolePointerTopPosition = Console.GetCursorPosition().Top;
        Console.SetCursorPosition(0, consolePointerTopPosition);

        var dateTimeTemplate = DateTime.Now.ToString("dd.MM HH:mm:ss");
        var timeColor = ConsoleColor.DarkGray;

        Console.ForegroundColor = timeColor;
        Console.Write($"[{dateTimeTemplate}] ");

        Console.ForegroundColor = senderColor;
        Console.Write($"[{sender}] ");

        Console.ForegroundColor = handlingMessageColor;
        Console.Write($"{handlingMessage} ");

        Console.ForegroundColor = defaultForeGroundColor;
        Console.WriteLine($"{message}");

    }
}