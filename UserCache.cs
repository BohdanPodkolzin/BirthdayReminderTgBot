using PRTelegramBot.Models;
using PRTelegramBot.Interface;

namespace tg
{
    public class UserCache : ITelegramCache
    {
        public List<DateTime>? CachedDates { get; set; }
        public string? PersonName { get; set; }
        public string? DateT { get; set; }
        public string? ChosenDate { get; set; }
        public bool ClearData()
        {
            PersonName = null;
            DateT = null;
            ChosenDate = null;
            CachedDates = new List<DateTime>();
            return true;
        }
    }
}