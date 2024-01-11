using PRTelegramBot.Models;

namespace tg
{
    public class UserCache : TelegramCache, ITelegramCache
    {
        public DateTime CachedDate { get; set; }
        public bool ClearData()
        {
            CachedDate = DateTime.MinValue;
            return true;
        }
    }
}