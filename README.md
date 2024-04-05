# Telegram.Bot Polling Example

## About

Telegram Bot using long polling 
([wiki](https://en.wikipedia.org/wiki/Push_technology#Long_polling)).

This example utilize [Worker Service](https://docs.microsoft.com/en-us/dotnet/core/extensions/workers)
template for hosting Bot application. This approach gives you such benefits as:

- cross-platform hosting;
- configuration;
- dependency injection (DI);
- logging;
- docker containerization;


## Prerequisites

Please make sure you have .NET 6 or newer installed. You can download .NET runtime from the [official site.](https://dotnet.microsoft.com/download)

You have to add [Telegram.Bot](https://www.nuget.org/packages/Telegram.Bot/) 
nuget package to your project to be able to use polling:

```shell
dotnet add package Telegram.Bot
```

Make sure that your .csproj contains these items (versions may vary):

```xml
<ItemGroup>
  <PackageReference Include="Telegram.Bot" Version="19.0.0" />
</ItemGroup>
```

## Configuration

Since telegram bot is containerized you need to pass bot token in it as environment variable.
 You can set variable name in `appsettings*.json`  file in BotConfiguration section. in order to do this you have to replace `{ENV_VARIABLE_CONTAINING_BOT_TOKEN}` with actual
environment variable that holds bot token:
```json
"BotConfiguration": {
  "TokenName": "{ENV_VARIABLE_CONTAINING_BOT_TOKEN}"
}
```
Its value should be stored in token.env file in name=value format. This file is ignored by docker and git.

