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
    public class ReminderBack
    {
        [ReplyMenuHandler("Test")]
        public static async Task RemindPersonForBirthday(ITelegramBotClient botClient, Update update)
        {
            DateTime currDate = DateTime.Today;
            var cache = update.GetCacheData<UserCache>();
            Message _ = await PRTelegramBot.Helpers.Message.Send(botClient, update, "Test");

            //foreach (var user in cache.scheduleDict)
            //{
            //    if (user.Value.Month.Equals(currDate.Month) && user.Value.Day.Equals(currDate.Day))
            //    {
            //        string message = $"Today is {user.Key}'s birthday!";
            //        Message _ = await PRTelegramBot.Helpers.Message.Send(botClient, update, message);
            //    }
            //}
        }
    }
}
