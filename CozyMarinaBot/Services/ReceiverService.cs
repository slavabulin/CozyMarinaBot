using Telegram.Bot;
using Telegram.Bot.Abstract;

namespace CozyMarinaBot.Services;

// Compose Receiver and UpdateHandler implementation
internal class ReceiverService : ReceiverServiceBase<UpdateHandler>
{
    public ReceiverService(
        ITelegramBotClient botClient,
        UpdateHandler updateHandler,
        ILogger<ReceiverServiceBase<UpdateHandler>> logger)
        : base(botClient, updateHandler, logger)
    {
    }
}
