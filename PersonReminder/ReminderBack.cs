using BirthdayReminder.Telegram.Models;
using PRTelegramBot.Extensions;
using Telegram.Bot;
using Telegram.Bot.Types;
using static BirthdayReminder.DataBase.DataBaseConnector.MySqlConnector;

namespace BirthdayReminder.PersonReminder
{
    public static class ReminderBack
    {
        public static async Task RemindPersonForBirthday(ITelegramBotClient botClient, Update update)
        {
            var currDate = DateTime.Today;

            if (update.Message?.From == null)
            {
                return;
            }
            var dataFromDataBase = await GetData(update.Message.From.Id);

            foreach (var person in dataFromDataBase)
            {
                if (person.BirthdayDate.Month.Equals(currDate.Month) && person.BirthdayDate.Day.Equals(currDate.Day))
                {
                    var message = $"Today is {person.Name}'s birthday!";
                    _ = await PRTelegramBot.Helpers.Message.Send(botClient, update, message);
                }
                else if (person.BirthdayDate.Month.Equals(currDate.Month) && person.BirthdayDate.Day.Equals(currDate.Day + 1))
                {
                    var message = $"{person.Name}'s birthday is tomorrow!";
                    _ = await PRTelegramBot.Helpers.Message.Send(botClient, update, message);
                }
            }
        }
    }
}
