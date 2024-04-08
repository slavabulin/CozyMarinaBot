using CozyMarinaBot.BLL;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace CozyMarinaBot.Services;

internal class UpdateHandler : IUpdateHandler
{
    private readonly IBlService _blService;

    public UpdateHandler(IBlService blService)
    {
        _blService = blService;
    }

    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        await _blService.HandleUpdateAsync(botClient, update, cancellationToken);
    }
    public Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
