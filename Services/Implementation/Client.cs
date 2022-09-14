using Commands.Abstraction;
using Core.Abstraction.Interfaces;
using Domain.Abstraction.Interfaces;
using Services.Abstraction;
using Services.Intefraces;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Services.Implementation;

public class Client : IClient
{
    private readonly IShowMessage _showMessage;
    private readonly ITelegramBotClient _telegramBotClient;
    private readonly IUserService _userService;
    private readonly ITelegramDataParser _telegramDataParser;
    private readonly Func<string, Command> _commandFactory;
    private readonly Func<string, KeyboardMarkup> _keyboardFactory;
    private IList<string> _commandArgs;

    public Client(IShowMessage showMessage,
        IUserService userService,
        Func<string, Command> commandFactory,
        Func<string, KeyboardMarkup> keyboardFactory,
        ITelegramDataParser telegramDataParser)
    {
        _commandArgs = new List<string>();
        _showMessage = showMessage;
        _userService = userService;
        _commandFactory = commandFactory;
        _keyboardFactory = keyboardFactory;
        _telegramBotClient = new TelegramBotClient("5465882205:AAHeJxcj-nMNygQxYprB7oBr33O1Qi3iNoo");
        _telegramDataParser = telegramDataParser;
    }
    private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
        {
            var message = update.Message;

            var response = _telegramDataParser.GetResponse(message);
            if (response == null)
            {
                return;
            }

            var user = await _userService.GetByTelegramIdAsync(response.FromId);

            var isAuthorized = user != null;

            if (isAuthorized)
            {
                var commandArgs = message?.Text ?? string.Empty;

                var command = _commandFactory(commandArgs);
                command.ImportCommandArgs(commandArgs, '-');
                var commandResult = await command.RunCommandAsync();

                _showMessage.ShowInfo($"{response.FromId} sent command \"{commandArgs}\" with result \"{commandResult.Message}\" developer:({commandResult.IsSuccessful})");

                var keyboardMarkup = _keyboardFactory("options");
                keyboardMarkup.ImportResponse(response);
                await SendMessageAsync(message, commandResult.Message, keyboardMarkup.Keyboard);
            }
            else
            {
                var keyboardMarkup = _keyboardFactory("reg");
                keyboardMarkup.ImportResponse(response);
                await SendMessageAsync(message, "Please sign in)", keyboardMarkup.Keyboard);
                
            }
            
        }
        else if (update.Type == Telegram.Bot.Types.Enums.UpdateType.CallbackQuery)
        {
            await ReceiveCallbackQueryAsync(update.CallbackQuery);
        }
    }

    private async Task SendMessageAsync(Message message, string text, InlineKeyboardMarkup markup = null)
    {
        await _telegramBotClient.SendTextMessageAsync(message.Chat, text, replyMarkup:markup);
    }

    private async Task ReceiveCallbackQueryAsync(CallbackQuery callbackQuery)
    {
        //reg id
        //delete id
        //get id
        //get_account id id
        //auto_account property

        //add_account login password
        //remove_account login
        //edit_account property value
        //get_accounts id
        var response = _telegramDataParser.GetResponse(callbackQuery.Message, callbackQuery.Data);
        if (response == null)
        {
            return;
        }

        var command = _commandFactory(response.CommandArgs);
        command.ImportCommandArgs(response.CommandArgs, response.Separator);
        var commandResult = await command.RunCommandAsync();
        await SendMessageAsync(callbackQuery.Message, $"{commandResult.Message}");

    }

    private async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        var jsonException = Newtonsoft.Json.JsonConvert.SerializeObject(exception);
        _showMessage.ShowError(jsonException);
    }

    public void Initialize()
    {

        _showMessage.ShowInfo("Started Telegram bot");

        var cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = cancellationTokenSource.Token;
        var receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = { }, // receive all update types
        };
        _telegramBotClient.StartReceiving(
            HandleUpdateAsync,
            HandleErrorAsync,
            receiverOptions,
            cancellationToken
        );
    }
}