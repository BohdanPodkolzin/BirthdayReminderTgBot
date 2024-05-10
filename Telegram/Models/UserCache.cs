using PRTelegramBot.Interface;

namespace BirthdayReminder.Telegram.Models
{
    public class UserCache : ITelegramCache
    {
        public readonly Dictionary<string, DateTime> ScheduleDict = new();
        public string? PersonName { get; set; }

        public bool ClearData()
        {
            ScheduleDict.Clear();
            PersonName = null;
            return true;
        }
    }
}
