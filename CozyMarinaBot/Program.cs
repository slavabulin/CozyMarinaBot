using CozyMarinaBot.BLL;
using CozyMarinaBot.DAL.Repositories;
using CozyMarinaBot.Services;
using Telegram.Bot;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        // Register Bot
        services.Configure<BotConfiguration>(
            context.Configuration.GetSection(BotConfiguration.Configuration));

        // Register named HttpClient to benefits from IHttpClientFactory
        // and consume it with ITelegramBotClient typed client.
        // More read:
        //  https://docs.microsoft.com/en-us/aspnet/core/fundamentals/http-requests?view=aspnetcore-5.0#typed-clients
        //  https://docs.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests
        services.AddHttpClient("telegram_bot_client")
                .AddTypedClient<ITelegramBotClient>((httpClient, sp) =>
                {
                    BotConfiguration? botConfig = sp.GetConfiguration<BotConfiguration>();
                    var token = Environment.GetEnvironmentVariable(botConfig.TokenName) ?? string.Empty;
                    TelegramBotClientOptions options = new(token);
                    return new TelegramBotClient(options, httpClient);
                });

        services.AddScoped<UpdateHandler>();
        services.AddScoped<ReceiverService>();
        services.AddHostedService<PollingService>();
        services.AddSingleton<IBearService, BearService>();
        services.AddScoped<IWordsService, WordsService>();
        services.AddScoped<IWordsRepo, WordsRepo>();
        services.AddScoped<IUsersService, UsersService>();
        services.AddScoped<IUsersRepo, UsersRepo>();
        services.AddScoped<IBlService, BlService>();
    })
    .Build();

await host.RunAsync();
