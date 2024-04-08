using Telegram.Bot;
using Telegram.Bot.Types;

namespace CozyMarinaBot.BLL
{
    interface IBlService
    {
        Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken);
    }
}