using PRTelegramBot.Core;

const string exit = "exit";

var tgBot = new PRBot(option =>
{
    option.BotId = 0;
    option.Admins = [];
    option.WhiteListUsers = [];
    option.Token = "TG_BOT_TOKEN";
    option.ClearUpdatesOnStart = true;
});

tgBot.OnLogCommon += TgBotOnLogCommon;
tgBot.OnLogError += TgBotOnLogError;

await tgBot.Start();

while (true)
{
    var result = Console.ReadLine();
    if (result?.ToLower() == exit)
    {
        Environment.Exit(0);
    }
}


return;

static void TgBotOnLogCommon(string msg, Enum? eventTypes, ConsoleColor color)
{
    Console.ForegroundColor = ConsoleColor.Yellow;
    var commonMsg = $"{DateTime.Now}:{msg}";
    Console.WriteLine(commonMsg);
    Console.ResetColor();
}

static void TgBotOnLogError(Exception ex, long? chatId)
{
    Console.ForegroundColor = ConsoleColor.Red;
    var errorMsg = $"{DateTime.Now}:{ex.Message}";
    Console.WriteLine(errorMsg);
    Console.ResetColor();
}
