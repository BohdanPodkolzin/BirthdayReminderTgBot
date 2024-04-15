using PRTelegramBot.Attributes;
using PRTelegramBot.Extensions;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;
using tg.Models;


namespace tg.UsersCache
{
    public class CacheCommand
    {
        [ReplyMenuHandler("Show Countdown")]
        public static async Task CheckCache(ITelegramBotClient botClient, Update update)
        {
            var cache = update.GetCacheData<UserCache>();

            StringBuilder message = new StringBuilder();
            if (cache.scheduleDict.Count > 0)
            {
                message.Append("<b>Birthdays schedule</b>\n");
                foreach (var user in cache.scheduleDict)
                {
                    int daysUntilBirthday = GetDaysUntilBirthday(user.Value);
                    message.AppendLine($"\n· <b>{user.Key}</b>, {user.Value.ToString("dd.MM.yyyy")} {(daysUntilBirthday.Equals(0) ? "<b>birthday is today!</b>" : $"until birthday: <b>{daysUntilBirthday}</b>")}");
                }
            }
            else
            {
                message.Append("<b>Your schedule is empty</b>.");
            }

            Message _ = await PRTelegramBot.Helpers.Message.Send(botClient, update, message.ToString());
        }


        public static async Task UpdateCache(Update update, string name, DateTime date)
        {
            var cache = update.GetCacheData<UserCache>();

            if (!cache.scheduleDict.ContainsKey(name))
            {
                cache.scheduleDict.Add(name, date);
            }
            else
            {
                cache.scheduleDict[name] = date;
            }
        }

        private static int GetDaysUntilBirthday(DateTime birthday)
        {
            DateTime currentDate = DateTime.Today;
            DateTime birthdayThisYear = new DateTime(currentDate.Year, birthday.Month, birthday.Day);

            if (birthdayThisYear < currentDate)
            {
                birthdayThisYear = birthdayThisYear.AddYears(1);
            }

            TimeSpan difference = birthdayThisYear - currentDate;
            int daysUntilBirthday = (int)difference.TotalDays;

            return daysUntilBirthday;
        }
    }

}