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
        private struct LastCheckedDateInfo
        {
            public DateTime LastCheckedDate;
        }

        private static readonly Dictionary<int, LastCheckedDateInfo> _lastCheckedDates = new Dictionary<int, LastCheckedDateInfo>();
        private static readonly TimeSpan _checkInterval = TimeSpan.FromSeconds(3); 

        public static async Task StartReminderLoop(ITelegramBotClient botClient, Update update)
        {
            int userId = (int)update.Message.From.Id;

            for (;;)
            {
                if (!_lastCheckedDates.TryGetValue(userId, out LastCheckedDateInfo info))
                {
                    await ReminderBack.RemindPersonForBirthday(botClient, update);
                    _lastCheckedDates[userId] = new LastCheckedDateInfo { LastCheckedDate = DateTime.Today };
                }
                await Task.Delay(_checkInterval);
            }
        }


    }
}