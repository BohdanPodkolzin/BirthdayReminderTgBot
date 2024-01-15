using System;
using PRTelegramBot;
using PRTelegramBot.Core;
using tg;

const string EXIT = "exit";


var tgBot = new PRBot(option =>
{
    option.Token = BotCfg.Token;
    option.Admins = new List<long> { };
    option.WhiteListUsers = new List<long> { };
    option.ClearUpdatesOnStart = true;
    option.BotId = 0;
});

tgBot.OnLogCommon += TgBot_OnLogCommon;
tgBot.OnLogError += TgBot_OnLogError;

await tgBot.Start();
static void TgBot_OnLogError(Exception ex, long? chatId)
{
    Console.ForegroundColor = ConsoleColor.Red;
    string errorMsg = $"{DateTime.Now}:{ex.Message}";
    Console.WriteLine(errorMsg);
    Console.ResetColor();
}

static void TgBot_OnLogCommon(string msg, Enum? eventTypes, ConsoleColor color)
{
    Console.ForegroundColor = ConsoleColor.Yellow;
    string commonMsg = $"{DateTime.Now}:{msg}";
    Console.WriteLine(commonMsg);
    Console.ResetColor();
}

while (true)
{
    var result = Console.ReadLine();
    if (result?.ToLower() == EXIT)
    {
        Environment.Exit(0);
    }
}