using PRTelegramBot.Models;
using PRTelegramBot.Interface;

namespace tg.UsersCache
{
    public class UserCache : ITelegramCache
    {
        public Dictionary<string, DateTime> scheduleDict = new Dictionary<string, DateTime>();
        public List<DateTime>? CachedDates { get; set; }
        public string? PersonName { get; set; }
        public DateTime DateT { get; set; }
        public bool ClearData()
        {
            scheduleDict.Clear();
            PersonName = null;
            DateT = DateTime.MinValue;
            CachedDates = new List<DateTime>();
            return true;
        }
    }
}