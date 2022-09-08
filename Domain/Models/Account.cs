namespace Domain.Models;

public class Account
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
}