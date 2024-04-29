using PRTelegramBot.Interface;

namespace BirthdayReminder.UsersCache
{
    public class UserCache : ITelegramCache
    {
        public Dictionary<string, DateTime> scheduleDict = new();
        public string? PersonName { get; set; }
        public DateTime DateT { get; set; }

        public bool ClearData()
        {
            scheduleDict.Clear();
            PersonName = null;
            DateT = DateTime.MinValue;
            return true;
        }
    }
}
