using PRTelegramBot.Models;
using PRTelegramBot.Interface;

namespace tg
{
    public class UserCache : ITelegramCache
    {
        public List<DateTime>? CachedDates { get; set; }
        public string? PersonName { get; set; }
        public string? DateT { get; set; }
        public bool ClearData()
        {
            PersonName = string.Empty;
            DateT = string.Empty;
            CachedDates = new List<DateTime>();
            return true;
        }
    }
}