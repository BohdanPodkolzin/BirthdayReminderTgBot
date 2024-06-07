using Telegram.Bot;
using Telegram.Bot.Types;
using static BirthdayReminder.DataBase.DataBaseConnector.Queries;

namespace BirthdayReminder.PersonReminder
{
    public static class ReminderBack
    {
        public static async Task RemindPersonForBirthday(ITelegramBotClient botClient, long update)
        {
            var currDate = DateTime.Today;

            var dataFromDataBase = await GetData(update);

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
