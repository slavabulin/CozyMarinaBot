using Telegram.Bot.Abstract;

namespace CozyMarinaBot.Services;

// Compose Polling and ReceiverService implementations
internal class PollingService : PollingServiceBase<ReceiverService>
{
    public PollingService(IServiceProvider serviceProvider, ILogger<PollingService> logger)
        : base(serviceProvider, logger)
    {
    }
}
