using System.Xml.Serialization;
using Microsoft.Extensions.DependencyInjection;
using Services.Implementation.Domain.Models;
using Telegram.Bot.Types.ReplyMarkups;

namespace Services.Abstraction;

public abstract class KeyboardMarkup
{
    protected abstract string Name { get; }

    public abstract InlineKeyboardMarkup Keyboard { get; }
    protected TelegramResponse Response { get; set; }

    protected virtual bool IsKeyboardFor(string name)
    {
        return Name == name;
    }

    public virtual void ImportResponse(TelegramResponse response)
    {
        Response = response;
    }
    public static Func<IServiceProvider, Func<string, KeyboardMarkup>> GetKeyboardMarkup =>
        provider => input =>
        {
            var keyboardMarkup = provider.GetServices<KeyboardMarkup>().First(c => c.IsKeyboardFor(input));

            return keyboardMarkup;
        };
}