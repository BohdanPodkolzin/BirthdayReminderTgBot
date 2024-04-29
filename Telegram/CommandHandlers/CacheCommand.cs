using System.Text;
using PRTelegramBot.Attributes;
using PRTelegramBot.Extensions;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BirthdayReminder.Telegram.CommandHandlers
{
    public static class CacheCommand
    {
        [ReplyMenuHandler("Show Countdown")]
        public static async Task CheckCache(ITelegramBotClient botClient, Update update)
        {
            var cache = update.GetCacheData<UserCache>();

            var messageBuilder = new StringBuilder();
            if (cache.ScheduleDict.Count <= 0)
            {
                messageBuilder.Append("<b>Your schedule is empty</b>.");
            }
            else
            {
                messageBuilder.Append("<b>Birthdays schedule</b>\n");
                foreach (var user in cache.ScheduleDict)
                {
                    var daysUntilBirthday = GetDaysUntilBirthday(user.Value);
                    messageBuilder
                        .AppendLine()
                        .AppendLine($"· <b>{user.Key}</b>, " +
                                    $"{user.Value.ToString("dd.MM.yyyy")} " +
                                    $"{GetCountdownPart(daysUntilBirthday)}"
                        );
                }

                string GetCountdownPart(int daysUntilBirthday)
                    => daysUntilBirthday is 0
                        ? "<b>birthday is today!</b>"
                        : $"until birthday: <b>{daysUntilBirthday}</b>";
            }

            await PRTelegramBot.Helpers.Message.Send(botClient, update, messageBuilder.ToString());
        }

        public static void UpdateCache(Update update, string name, DateTime date)
        {
            var cache = update.GetCacheData<UserCache>();
            cache.ScheduleDict[name] = date;
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
