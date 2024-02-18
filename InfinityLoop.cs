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
        
        private static readonly TimeSpan _checkInterval = TimeSpan.FromSeconds(3);

        private static Dictionary<int, DateTime> checkedDates = new Dictionary<int, DateTime>();

        public static async Task StartReminderLoop(ITelegramBotClient botClient, Update update)
        {
            int userId = (int)update.Message.From.Id;
            checkedDates.Add(userId, DateTime.Today.AddDays(-1));

            for (;;)
            {
                foreach (int user in checkedDates.Keys)
                {
                    if (checkedDates[user].Day != DateTime.Today.Day)
                    {
                        checkedDates[user] = DateTime.Today;
                        await ReminderBack.RemindPersonForBirthday(botClient, update);
                    }
                }
                await Task.Delay(_checkInterval);
            }
        }


    }
}