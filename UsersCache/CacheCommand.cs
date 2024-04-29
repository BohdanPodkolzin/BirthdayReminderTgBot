using System.Text;
using PRTelegramBot.Attributes;
using PRTelegramBot.Extensions;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BirthdayReminder.UsersCache
{
    public class CacheCommand
    {
        [ReplyMenuHandler("Show Countdown")]
        public static async Task CheckCache(ITelegramBotClient botClient, Update update)
        {
            var cache = update.GetCacheData<UserCache>();

            var message = new StringBuilder();
            if (cache.scheduleDict.Count > 0)
            {
                message.Append("<b>Birthdays schedule</b>\n");
                foreach (var user in cache.scheduleDict)
                {
                    var daysUntilBirthday = GetDaysUntilBirthday(user.Value);
                    message.AppendLine($"\n· <b>{user.Key}</b>, {user.Value.ToString("dd.MM.yyyy")} {(daysUntilBirthday.Equals(0) ? "<b>birthday is today!</b>" : $"until birthday: <b>{daysUntilBirthday}</b>")}");
                }
            }
            else
            {
                message.Append("<b>Your schedule is empty</b>.");
            }

            var _ = await PRTelegramBot.Helpers.Message.Send(botClient, update, message.ToString());
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
            var currentDate = DateTime.Today;
            var birthdayThisYear = new DateTime(currentDate.Year, birthday.Month, birthday.Day);

            if (birthdayThisYear < currentDate)
            {
                birthdayThisYear = birthdayThisYear.AddYears(1);
            }

            var difference = birthdayThisYear - currentDate;
            var daysUntilBirthday = (int)difference.TotalDays;

            return daysUntilBirthday;
        }
    }

}
