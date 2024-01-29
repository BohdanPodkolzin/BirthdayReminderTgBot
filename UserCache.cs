using PRTelegramBot.Models;
using PRTelegramBot.Interface;

namespace tg
{
    public class UserCache : ITelegramCache
    {
        public Dictionary<string, DateTime> scheduleDict = new Dictionary<string, DateTime>();
        public List<DateTime>? CachedDates { get; set; }
        public string? PersonName { get; set; }
        public DateTime DateT { get; set; }
        public bool ClearData()
        {
            this.scheduleDict.Clear();
            this.PersonName = null;
            this.DateT = DateTime.MinValue;
            this.CachedDates = new List<DateTime>();
            return true;
        }
    }
}