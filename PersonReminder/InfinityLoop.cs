using System.ComponentModel.DataAnnotations;
using BirthdayReminder.DependencyInjectionConfiguration;
using static BirthdayReminder.MySqlDataBase.DataBaseConnector.Queries;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BirthdayReminder.PersonReminder
{
    public static class InfinityLoop
    {
        private static readonly SqlLoggerService? Logger = BotConfiguration.SqlLoggerService;
        private static readonly TimeSpan CheckInterval = TimeSpan.FromHours(1);

        private static bool _isRunning;
        private static readonly object Lock = new();

        public static async Task StartReminderLoop(ITelegramBotClient botClient, Update update)
        {
            //var userIds = await GetUsersIds();

            lock (Lock)
            {
                if (_isRunning)
                {
                    return;
                }

                _isRunning = true;
                Logger?.LogInfinityLoop();
            }

            while (_isRunning)
            {
                var placeCoordsObjectList = await GetUsersPlaceCoordinatesTable();
                foreach (var placeCoordsObject in placeCoordsObjectList)
                {
                    var currDate = await GetApiKeyDate(placeCoordsObject.Latitude, placeCoordsObject.Longitude);

                    Console.WriteLine(currDate);
                    Console.WriteLine(placeCoordsObject.TodayDate);
                    if (placeCoordsObject.TodayDate.Month.Equals(currDate.Month)
                        && placeCoordsObject.TodayDate.Day.Equals(currDate.Day)) continue;

                    await SetTodayDate(placeCoordsObject.TelegramId, placeCoordsObject.TodayDate.AddDays(1));
                    await ReminderBack.RemindPersonForBirthday(botClient, placeCoordsObject.TelegramId, placeCoordsObject.TodayDate.AddDays(1));
                }

                await Task.Delay(CheckInterval);
            }
        }
    }
}