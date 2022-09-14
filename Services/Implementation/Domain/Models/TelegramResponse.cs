namespace Services.Implementation.Domain.Models;

public class TelegramResponse
{
    public long FromId { get; set; }
    public long ChatId { get; set; }
    public string CommandArgs { get; set; }
    public char Separator { get; set; }
    public bool FromUser()
    {
        return FromId == ChatId;
    }
}