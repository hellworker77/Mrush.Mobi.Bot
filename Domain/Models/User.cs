namespace Domain.Models;

public class User
{
    public Guid Id { get; set; }
    public long TelegramId { get; set; }
    public IList<Account> Accounts { get; set; }
    public override string ToString()
    {
        return $"Accounts: {Accounts.Count}";
    }
}