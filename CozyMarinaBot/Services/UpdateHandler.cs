using CozyMarinaBot.DAL.Services;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace CozyMarinaBot.Services;

internal class UpdateHandler : IUpdateHandler
{
    private readonly ITelegramBotClient _botClient;
    private readonly ILogger<UpdateHandler> _logger;
    private readonly IWordsService _wordService;
    private readonly IUsersService _userService;
    private readonly IBearService _bearService;
    private Dictionary<long, ChatData> _chatData;

    private const string _alreadyMsg = "Игра уже запущена.";
    private const string _gameStoppedMsg = "Игра остановлена.";
    private const string _greetingsMsg = "Жми медведа!";
    private const string _hostNotAllowedToAnswerMsg = "Ведущий не может отвечать!";
    private const string _successMsg = " иди обниму! Красава!";
    private const string _secretWordMsg = "Загаданное слово: ";
    private const string _wrongHostMsg = "Не подглядывай, ты не ведущий!";

    public UpdateHandler(ITelegramBotClient botClient, IWordsService wordService, IUsersService userService, IBearService bearService, ILogger<UpdateHandler> logger)
    {
        _botClient = botClient;
        _logger = logger;
        _wordService = wordService;
        _userService = userService;
        _bearService = bearService;
        _chatData = new Dictionary<long, ChatData>();
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
        var id = message.Chat.Id;
        if (!_chatData.ContainsKey(id)) return message;

        if (_chatData[id].GameIsStarted
            && !string.IsNullOrWhiteSpace(message?.Text)
            && message?.Text.ToLower() == _chatData[id].SecretWord.ToLower())
        {
            if (message.From.Id == _chatData[id].HostId)
            {
                return await botClient.SendTextMessageAsync(
                    chatId: id,
                    text: _hostNotAllowedToAnswerMsg,
                    cancellationToken: cancellationToken);
            }
            _chatData[id].WordIsChosen = false;
            //save user's score
            var user = message?.From?.ToDalUser(message.Chat.Id);
            await _userService.IncrementUsersScoreAsync(user, cancellationToken);

            //change game's host
            _chatData[id].HostId = message?.From?.Id;

            //show word button
            _chatData[id].GameIsStarted = false;

            await botClient.SendAnimationAsync(
                chatId: message.Chat.Id,
                animation: Helper.GetGifStream(),
                caption: $"{message.From.Username ?? message.From.FirstName}{_successMsg}",
                cancellationToken: cancellationToken);

            return await StartGame(botClient, message, cancellationToken);
        }
        return new Message();
    }
    async Task<Message> StartGame(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {
        var id = message.Chat.Id;
        if (!_chatData.ContainsKey(message.Chat.Id))
        {
            _chatData.Add(message.Chat.Id, new ChatData { HostId = message?.From?.Id, GameIsStarted = false });
        }

        if (!_chatData[id].GameIsStarted)
        {
            _chatData[id].GameIsStarted = true;
            _chatData[id].HostId = message?.From?.Id ?? 0;

            InlineKeyboardMarkup inlineKeyboard = new(
                new[]{
                        new []{
                            InlineKeyboardButton.WithCallbackData(_bearService.GetNewBear(), "11")
                        }
                });

            return await botClient.SendTextMessageAsync(
                chatId: id,
                text: _greetingsMsg,
                replyMarkup: inlineKeyboard,
                cancellationToken: cancellationToken);
        }
        return await botClient.SendTextMessageAsync(
            chatId: id,
            text: _alreadyMsg,
            cancellationToken: cancellationToken);
    }

    async Task<Message> StopGame(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {
        var id = message.Chat.Id;
        if (!_chatData.ContainsKey(message.Chat.Id)) return message;
        _chatData[id].GameIsStarted = false;
        _chatData[id].WordIsChosen = true;
        _chatData[id].SecretWord = await _wordService.GetWordAsync();

        return await botClient.SendTextMessageAsync(
            chatId: id,
            text: _gameStoppedMsg,
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
        var id = callbackQuery.Message.Chat.Id;
        if (!_chatData.ContainsKey(id)) return;

        if(callbackQuery?.From.Id == _chatData[id].HostId)
        { 
            if (!_chatData[id].WordIsChosen)
            {
                _chatData[id].WordIsChosen = true;
                _chatData[id].SecretWord = await _wordService.GetWordAsync();
            }

            await _botClient.AnswerCallbackQueryAsync(
            callbackQueryId: callbackQuery.Id,
            text: $"{_secretWordMsg}{_chatData[id].SecretWord}",
            cancellationToken: cancellationToken);
        }
        else
        {
            await _botClient.AnswerCallbackQueryAsync(
            callbackQueryId: callbackQuery.Id,
            text: _wrongHostMsg,
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
