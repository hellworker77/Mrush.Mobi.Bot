using Services.Implementation.Domain.Models;
using Telegram.Bot.Types;

namespace Services.Intefraces;

public interface ITelegramDataParser
{
    public TelegramResponse? GetResponse(Message? message, string? rawData = null);
}