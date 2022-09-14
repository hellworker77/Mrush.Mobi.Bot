using Services.Abstraction;
using Telegram.Bot.Types.ReplyMarkups;

namespace Services.Implementation.Domain.Keyboards;

public class KeyboardMarkupSignIn : KeyboardMarkup
{
    protected override string Name => "reg";
    public override InlineKeyboardMarkup Keyboard => new(new[]
    {
        new[]
        {
            InlineKeyboardButton.WithCallbackData("SignIn",$"reg {Response.ChatId}")
        }
    });
}