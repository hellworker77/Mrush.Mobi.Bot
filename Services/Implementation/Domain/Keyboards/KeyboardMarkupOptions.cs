using Services.Abstraction;
using Telegram.Bot.Types.ReplyMarkups;

namespace Services.Implementation.Domain.Keyboards;

public class KeyboardMarkupOptions : KeyboardMarkup
{
    protected override string Name => "options";
    public override InlineKeyboardMarkup Keyboard => new(new[]
    {
        new[]
        {
            InlineKeyboardButton.WithCallbackData("Delete",$"delete {Response.ChatId}"),
            
        },
        new[]
        {
            InlineKeyboardButton.WithCallbackData("Info",$"get-user {Response.ChatId}"),
            InlineKeyboardButton.WithCallbackData("Accounts",$"get-accounts {Response.ChatId}"),
        }
    });
}