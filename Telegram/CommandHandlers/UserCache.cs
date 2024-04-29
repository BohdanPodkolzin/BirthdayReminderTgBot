using PRTelegramBot.Interface;

namespace BirthdayReminder.Telegram.CommandHandlers
{
    public class UserCache : ITelegramCache
    {
        public readonly Dictionary<string, DateTime> ScheduleDict = new();
        public string? PersonName { get; set; }
        public DateTime DateT { get; set; }

        public bool ClearData()
        {
            ScheduleDict.Clear();
            PersonName = null;
            DateT = DateTime.MinValue;
            return true;
        }
    }
}
