using Telegram.Bot;
using Telegram.Bot.Types;
using static BirthdayReminder.MySqlDataBase.DataBaseConnector.Queries;

namespace BirthdayReminder.PersonReminder
{
    public static class ReminderBack
    {
        public static async Task RemindPersonForBirthday(ITelegramBotClient botClient, long userId, DateTime currentDay)
        {

            var dataFromDataBase = await GetFullRecordsData(userId);

            foreach (var record in dataFromDataBase)
            {
                if (record.BirthdayDate == null)
                {
                    Console.WriteLine("Error at if");
                    return;
                };

                var tomorrow = currentDay.AddDays(1);
                var message = string.Empty;
                if (record.BirthdayDate.Value.Month.Equals(currentDay.Month) &&
                    record.BirthdayDate.Value.Day.Equals(currentDay.Day))
                {
                    message = $"Today is {record.Name}'s birthday";
                }
                else if (record.BirthdayDate.Value.Month.Equals(tomorrow.Month) &&
                         record.BirthdayDate.Value.Day.Equals(tomorrow.Day))
                {
                    message = $"{record.Name}'s birthday is tomorrow!";
                }

                _ = await PRTelegramBot.Helpers.Message.Send(botClient, userId, message);
            }
        }
    }
}
