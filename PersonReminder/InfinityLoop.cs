using Telegram.Bot;
using Telegram.Bot.Types;

namespace BirthdayReminder.PersonReminder
{
    public static class InfinityLoop
    {
        private static readonly TimeSpan CheckInterval = TimeSpan.FromSeconds(10);

        private static DateTime _lastCheckDate = DateTime.UtcNow;

        public static async Task StartReminderLoop(ITelegramBotClient botClient, Update update)
        {
            while (true)
            {
                //if (_lastCheckDate.Day != DateTime.UtcNow.Day)
                //{
                    await ReminderBack.RemindPersonForBirthday(botClient, update);
                    _lastCheckDate = DateTime.UtcNow;
                //}
                await Task.Delay(CheckInterval);
            }
        }
    }
}
