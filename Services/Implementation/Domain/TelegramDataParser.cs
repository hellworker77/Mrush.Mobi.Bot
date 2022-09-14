using Services.Implementation.Domain.Models;
using Services.Intefraces;
using Telegram.Bot.Types;

namespace Services.Implementation.Domain;

public class TelegramDataParser : ITelegramDataParser
{
    public TelegramDataParser()
    {
        
    }

    public TelegramResponse? GetResponse(Message? message, string? rawData = null)
    {
        var response = new TelegramResponse();
        if (message == null)
        {
            return null;
        }

        var dataString = rawData ?? message.Text ?? string.Empty;
        var separator = GetSeparatorFromData(dataString);

        var data = GetDataFromRawData(dataString, separator);

        response.FromId = message.From?.Id ?? 0;
        response.ChatId = message.Chat.Id;
        response.Separator = separator;
        response.CommandArgs = data;

        return response;
    }
    private string GetDataFromRawData(string rawData, char separator)
    {
        if (separator == ' ')
        {
            return rawData;
        }
        var parsedData = rawData.Split(' ').ToList();


        parsedData.RemoveAt(parsedData.Count - 1);

        var data = string.Join(" ", parsedData);

        return data;
    }
    private char GetSeparatorFromData(string rawData)
    {
        var separator = ' ';
        string[] parsedData = rawData.Split(' ');
        var lastString = parsedData.LastOrDefault();

        if (lastString != null)
        {
            if (lastString.Length == 1)
            {
                separator = lastString[0];
            }
        }

        return separator;
    }
}