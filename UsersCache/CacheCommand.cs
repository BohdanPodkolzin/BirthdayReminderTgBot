using PRTelegramBot.Attributes;
using PRTelegramBot.Extensions;
using PRTelegramBot.Models.InlineButtons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using tg.Models;


namespace tg.UsersCache
{
    public class CacheCommand
    {
        public static async Task UpdateCache(Update update, string name, DateTime date)
        {
            var cache = update.GetCacheData<UserCache>();

            if (!cache.scheduleDict.ContainsKey(name))
            {
                cache.scheduleDict.Add(name, date);
            }
            else
            {
                cache.scheduleDict[name] = date;
            }
        }


        [ReplyMenuHandler("Show Countdown")]
        public static async Task CheckCache(ITelegramBotClient botClient, Update update)
        {
            var cache = update.GetCacheData<UserCache>();

            string message;
            if (cache.scheduleDict.Count > 0)
            {
                message = "Users in the cache:";
                foreach (var user in cache.scheduleDict)
                {
                    message += $"\n- Name: {user.Key}, Date: {user.Value.ToString("dd.MM.yyyy")}";
                }
            }
            else
            {
                message = "No users in the cache.";
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