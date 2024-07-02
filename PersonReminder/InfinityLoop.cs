using System.ComponentModel.DataAnnotations;
using static BirthdayReminder.MySqlDataBase.DataBaseConnector.Queries;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BirthdayReminder.PersonReminder
{
    public static class InfinityLoop
    {
        private static readonly TimeSpan CheckInterval = TimeSpan.FromSeconds(10);
        private static DateTime _lastCheckDate = DateTime.UtcNow;

        private static bool _isRunning;
        private static readonly object Lock = new();

        public static async Task StartReminderLoop(ITelegramBotClient botClient, Update update)
        {
            var userIds = await GetUsersIds();

            lock (Lock)
            {
                if (_isRunning)
                {
                    return;
                }

                _isRunning = true;
            }

            while (_isRunning)
            {
                foreach (var userId in userIds
                    .Where(_ => _lastCheckDate.Day != DateTime.UtcNow.Day))
                {
                    await ReminderBack.RemindPersonForBirthday(botClient, userId);
                    _lastCheckDate = DateTime.UtcNow;
                }

                await Task.Delay(CheckInterval);
            }
        }
    }
}