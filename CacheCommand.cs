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
            string message = $"Cache: {update.GetChatId()}";
            update.GetCacheData<UserCache>().Id = update.GetChatId();
            Message _ = await PRTelegramBot.Helpers.Message.Send(botClient, update, message);

        }


        [ReplyMenuHandler("resultcache")]
        public static async Task CheckCache(ITelegramBotClient botClient, Update update)
        {
            UserCache cache = update.GetCacheData<UserCache>();
            string message = "";
            if (cache.Id != 0)
            {
                message = $"Data in userCache: {cache.Id}";

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
