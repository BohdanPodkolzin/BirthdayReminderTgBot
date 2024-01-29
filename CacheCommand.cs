using PRTelegramBot.Attributes;
using PRTelegramBot.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using static tg.UserCache;


namespace tg
{
    public class CacheCommand
    {
        public static Dictionary<string, DateTime> nameDate = new Dictionary<string, DateTime>();
        public static DateTime CheckDate(string name)
        {
            return nameDate[name];
        }

        [ReplyMenuHandler("Show Countdown")]
        public static async Task GetDatesCache(ITelegramBotClient botClient, Update update)
        {
            var cache = update.GetCacheData<UserCache>();

            nameDate.Add(cache.PersonName, cache.DateT);
            var rightdate = CheckDate(cache.PersonName);



            string message = $"Name of userrra {cache.PersonName}, date {rightdate}";


            //string message;
            //if (cache.CachedDates != null && cache.CachedDates.Any())
            //{
            //    message = $"\n• {string.Join($"\n• ", cache.CachedDates.Select(date => date.ToString("dd.MM.yyyy")))}";
            //}
            //else
            //{
            //    message = $"Cache: No cached date";
            //}

            Message _ = await PRTelegramBot.Helpers.Message.Send(botClient, update, message);

        }

        public static async Task CheckCache(ITelegramBotClient botClient, Update update)
        {
            UserCache cache = update.GetCacheData<UserCache>();

            string message;
            if (cache.PersonName != null)
            {
                message = $"Name of user {cache.PersonName}, date {cache.DateT}";
            }
            else
            {
                message = "No datka";
            }
            Message _ = await PRTelegramBot.Helpers.Message.Send(botClient, update, message);

        }

        [ReplyMenuHandler("clearcache")]
        [InlineCallbackHandler<EditCountdownTHeader>(EditCountdownTHeader.AllDel)]
        public static async Task ClearCache(ITelegramBotClient botClient, Update update)
        {
            string message = "Cache cleared!";
            update.GetCacheData<UserCache>().ClearData();
            Message _ = await PRTelegramBot.Helpers.Message.Send(botClient, update, message);
        }


    }
}