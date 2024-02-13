using PRTelegramBot.Attributes;
using PRTelegramBot.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using tg.UsersCache;

namespace tg
{
    public class InfinityLoop
    {
        private static DateTime _lastCheckedDate = DateTime.Today.AddDays(-1);
        private static readonly TimeSpan _checkInterval = TimeSpan.FromMinutes(30); // Every 12 hours check again

        public static async Task StartReminderLoop(ITelegramBotClient botClient, Update update)
        {
            for (;;)
            {
                if (_lastCheckedDate != DateTime.Today)
                {
                    await ReminderBack.RemindPersonForBirthday(botClient, update);
                    _lastCheckedDate = DateTime.Today;
                }
                await Task.Delay(_checkInterval);
            }
        }


    }
}
