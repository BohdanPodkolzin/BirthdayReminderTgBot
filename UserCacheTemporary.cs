using PRTelegramBot.Models;
using PRTelegramBot.Interface;

namespace tg
{
    public class UserCacheTemporary : ITelegramCache
    {
        public string? Date { get; set; }
        public string? PersonName { get; set; }
        public bool ClearData()
        {
            this.PersonName = string.Empty;
            this.Date = string.Empty;
            return true;
        }
    }
}