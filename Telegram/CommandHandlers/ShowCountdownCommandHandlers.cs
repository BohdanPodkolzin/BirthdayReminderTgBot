using System.Text;
using BirthdayReminder.Telegram.Models;
using PRTelegramBot.Attributes;
using PRTelegramBot.Extensions;
using Telegram.Bot;
using Telegram.Bot.Types;
using static BirthdayReminder.DataBase.DataBaseConnector.MySqlConnector;

namespace BirthdayReminder.Telegram.CommandHandlers
{
    public static class ShowCountdownCommandHandlers
    {
        [ReplyMenuHandler("Show Countdown")]
        public static async Task ShowCountdownMethod(ITelegramBotClient botClient, Update update)
        {
            if (update.Message?.From == null)
            {
                return;
            }

            await RemoveInvalidRecords();

            var messageBuilder = new StringBuilder();
            if (await IsUserScheduleEmpty(update.Message.From.Id))
            {
                messageBuilder.Append("<b>Your schedule is empty</b>.");
            }
            else
            {
                var dataFromDataBase = await GetData(update.Message.From.Id);
                messageBuilder.Append("<b>Birthdays schedule</b>\n");

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
