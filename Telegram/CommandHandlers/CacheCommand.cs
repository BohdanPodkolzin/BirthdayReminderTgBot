using System.Text;
using BirthdayReminder.Telegram.Models;
using MySqlConnector;
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
            if (await DataBase.DataBaseConnector.MySqlConnector.IsUserScheduleEmpty(update.Message.From.Id))
            {
                messageBuilder.Append("<b>Your schedule is empty</b>.");
            }
            else
            {
                messageBuilder.Append("<b>Birthdays schedule</b>\n");

                var dataFromDataBase = await DataBase.DataBaseConnector.MySqlConnector.GetData(update.Message.From.Id);

                foreach (var person in dataFromDataBase)
                {
                    var daysUntilBirthday = GetDaysUntilBirthday(person.BirthdayDate);
                    messageBuilder
                        .AppendLine()
                        .AppendLine($"· <b>{person.Name}</b>, " +
                                    $"{person.BirthdayDate.ToString("dd.MM.yyyy")} " +
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
