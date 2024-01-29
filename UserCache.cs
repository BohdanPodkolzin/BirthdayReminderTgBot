using PRTelegramBot.Models;
using PRTelegramBot.Interface;

namespace tg
{
    public class UserCache : ITelegramCache
    {
        public List<DateTime>? CachedDates { get; set; }
        public string? PersonName { get; set; }
        public DateTime DateT { get; set; }
        public bool ClearData()
        {
            this.PersonName = null;
            this.DateT = DateTime.MinValue;
            this.CachedDates = new List<DateTime>();
            return true;
        }
    }
}