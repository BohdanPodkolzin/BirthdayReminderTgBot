using PRTelegramBot.Models;
using PRTelegramBot.Interface;

namespace tg
{
    public class UserCache : ITelegramCache
    {
        public List<DateTime>? CachedDates { get; set; }
        public string? PersonName { get; set; }
        public bool ClearData()
        {
            CachedDates = new List<DateTime>();
            return true;
        }
    }
}