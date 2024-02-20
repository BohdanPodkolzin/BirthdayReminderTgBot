using PRTelegramBot.Attributes;
using PRTelegramBot.Extensions;
using Telegram.Bot;
using Telegram.Bot.Types;
using tg.UsersCache;

namespace tg
{
    public class InfinityLoop
    {
        
        private static readonly TimeSpan _checkInterval = TimeSpan.FromSeconds(10);

        private static DateTime _lastCheckDate = DateTime.Now;

        public static async Task StartReminderLoop(ITelegramBotClient botClient, Update update)
        {
            for (;;)
            {
                if (_lastCheckDate.Day != DateTime.Now.Day)
                {
                    await ReminderBack.RemindPersonForBirthday(botClient, update);
                    _lastCheckDate = DateTime.Now;
                }
                await Task.Delay(_checkInterval);
            }
        }


    }
}