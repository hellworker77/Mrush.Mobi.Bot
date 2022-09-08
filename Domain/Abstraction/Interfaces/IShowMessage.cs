namespace Domain.Abstraction.Interfaces;

public interface IShowMessage
{
    public void ShowError(string message);
    public void ShowWarning(string message);
    public void ShowInfo(string message);
    public void ShowSuccessful(string message);
    public void ShowAsUser(string message);
    public void ShowAsConsole(string message);
}