using PRTelegramBot.Models;

namespace tg
{
    public class UserCache : TelegramCache, ITelegramCache
    {
        public List<DateTime> CachedDates { get; set; }
        public int CountDate { get; set; }
        public bool ClearData()
        {
            CachedDates = new List<DateTime>();
            return true;
        }
    }
}