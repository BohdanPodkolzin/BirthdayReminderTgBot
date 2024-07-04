using PRTelegramBot.Interface;

namespace BirthdayReminder.Telegram.Models
{
    public class RecordCache : ITelegramCache
    {
        public readonly Dictionary<string, DateTime> ScheduleDict = new();
        public string? PersonName { get; set; }

        public bool ClearData()
        {
            ScheduleDict.Clear();
            PersonName = string.Empty;
            return true;
        }
    }
}
