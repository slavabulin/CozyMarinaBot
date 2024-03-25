using CozyMarinaBot;
using CozyMarinaBot.DAL.Services;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Telegram.Bot.Services;

internal class UpdateHandler : IUpdateHandler
{
    private readonly ITelegramBotClient _botClient;
    private readonly ILogger<UpdateHandler> _logger;
    private readonly IWordsService _wordService;
    private readonly IUsersService _userService;
    private static string _secretWord = String.Empty;
    private static long _gameHost;
    private static bool _gameIsStarted;
    private static bool _wordIsChosen;

    public UpdateHandler(ITelegramBotClient botClient, IWordsService wordService, IUsersService userService, ILogger<UpdateHandler> logger)
    {
        _botClient = botClient;
        _logger = logger;
        _wordService = wordService;
        _userService = userService;
    }

    public async Task HandleUpdateAsync(ITelegramBotClient _, Update update, CancellationToken cancellationToken)
    {
        var handler = update switch
        {
            { Message: { } message }                        => BotOnMessageReceived(message, cancellationToken),
            { EditedMessage: { } message }                  => BotOnMessageReceived(message, cancellationToken),
            { CallbackQuery: { } callbackQuery }            => BotOnCallbackQueryReceived(callbackQuery, cancellationToken),
            _                                               => UnknownUpdateHandlerAsync(update, cancellationToken)
        };

        await handler;
    }
    private async Task BotOnMessageReceived(Message message, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Receive message type: {MessageType}", message.Type);
        if (message.Text is not { } messageText)
            return;

        var action = messageText.Split(' ', '@')[0] switch
        {
            "/start" => StartGame(_botClient, message, cancellationToken),
            "/stop"  => StopGame(_botClient, message, cancellationToken),
            "/stat"  => ShowStatistics(_botClient, message, cancellationToken),
            "/?"     => Usage(_botClient, message, cancellationToken),
            _        => CheckAnswer(_botClient, message, cancellationToken)
        };
        _ = await action;
    }
    async Task<Message> Usage(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {
        const string usage = "Usage:\n" +
                             "/start       - starts the game\n" +
                             "/stop        - stops the game\n" +
                             "/stat        - shows statistics\n" +
                             "/?           - shows this help\n";

        return await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: usage,
            replyMarkup: new ReplyKeyboardRemove(),
            cancellationToken: cancellationToken);
    }

    async Task<Message> CheckAnswer(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {
        if (_gameIsStarted
            && !string.IsNullOrWhiteSpace(message?.Text)
            && message?.Text.ToLower() == _secretWord.ToLower())
        {
            _wordIsChosen = false;
            //save user's score
            var user = message?.From?.ToDalUser(message.Chat.Id);
            await _userService.IncrementUsersScoreAsync(user, cancellationToken);

            //change game's host
            _gameHost = message.From.Id;

            //show word button
            _gameIsStarted = false;
            return await StartGame(botClient, message, cancellationToken);
        }
        return new Message();
    }
    async Task<Message> StartGame(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {
        if (!_gameIsStarted)
        {
            _gameIsStarted = true;
            _gameHost = message?.From?.Id ?? 0;

            InlineKeyboardMarkup inlineKeyboard = new(
                new[]{
                        new []{
                            InlineKeyboardButton.WithCallbackData("ʕᴥ• ʔ", "11")
                        }
                });

            return await botClient.SendTextMessageAsync(
                chatId: message?.Chat?.Id ?? 0,
                text: "Press button to get your word",
                replyMarkup: inlineKeyboard,
                cancellationToken: cancellationToken);
        }
        return new Message();
    }

    async Task<Message> StopGame(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {
        _gameIsStarted = false;
        return await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: "game stoped",
            replyMarkup: new ReplyKeyboardRemove(),
            cancellationToken: cancellationToken);
    }
    async Task<Message> ShowStatistics(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {
        var stats = _userService.GetStatistics(message.Chat.Id);
        return await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: stats,
            cancellationToken: cancellationToken);
    }

    private async Task BotOnCallbackQueryReceived(CallbackQuery callbackQuery, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Received inline keyboard callback from: {CallbackQueryId}", callbackQuery.Id);

        if(callbackQuery?.From.Id == _gameHost)
        { 
            if (!_wordIsChosen)
            {
                _wordIsChosen = true;
                _secretWord = await _wordService.GetWordAsync();
            }

            await _botClient.AnswerCallbackQueryAsync(
            callbackQueryId: callbackQuery.Id,
            text: $"Your word is: {_secretWord}",
            cancellationToken: cancellationToken);
        }
        else
        {
            await _botClient.AnswerCallbackQueryAsync(
            callbackQueryId: callbackQuery.Id,
            text: $"Sorry, you are not a host.",
            cancellationToken: cancellationToken);
        }
    }
    private Task UnknownUpdateHandlerAsync(Update update, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Unknown update type: {UpdateType}", update.Type);
        return Task.CompletedTask;
    }
    public Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
