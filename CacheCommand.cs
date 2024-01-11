using PRTelegramBot.Attributes;
using PRTelegramBot.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using static tg.UserCache;


namespace tg
{
    public class CacheCommand
    {
        [ReplyMenuHandler("cache")]
        public static async Task GetCacheData(ITelegramBotClient botClient, Update update)
        {
            var cache = update.GetCacheData<UserCache>();

            string message;

            if (cache.CachedDate != DateTime.MinValue)
            {
                message = $"Cached Date: {cache.CachedDate.ToString("dd.MM.yyyy")}";
            }
            else
            {
                message = $"Cache: No cached date";
            }

            Message _ = await PRTelegramBot.Helpers.Message.Send(botClient, update, message);

        }


        [ReplyMenuHandler("resultcache")]
        public static async Task CheckCache(ITelegramBotClient botClient, Update update)
        {
            UserCache cache = update.GetCacheData<UserCache>();

            string message;
            if (cache.CachedDate != DateTime.MinValue)
            {
                message = $"Data in userCache: {cache.CachedDate}";
            }
            else
            {
                message = "No any data";
            }
            Message _ = await PRTelegramBot.Helpers.Message.Send(botClient, update, message);

        }

        [ReplyMenuHandler("clearcache")]
        public static async Task ClearCache(ITelegramBotClient botClient, Update update)
        {
            string message = "Checking clear cache";
            update.GetCacheData<UserCache>().ClearData();
            Message _ = await PRTelegramBot.Helpers.Message.Send(botClient, update, message);
        }
    }
}