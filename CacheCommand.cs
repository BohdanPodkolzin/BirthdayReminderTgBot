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
        //public static Dictionary<string, DateTime> reminderDict = new Dictionary<string, DateTime>();
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


        public static async Task GetDatesCache(ITelegramBotClient botClient, Update update)
        {
            var cache = update.GetCacheData<UserCache>();

            await UpdateCache(update, cache.PersonName, cache.DateT);


            string message = $"User`s name {cache.PersonName}, date {cache.DateT}";
            //Message _ = await PRTelegramBot.Helpers.Message.Send(botClient, update, message);

        }

        [ReplyMenuHandler("Show Countdown")]
        public static async Task CheckCache(ITelegramBotClient botClient, Update update)
        {
            var cache = update.GetCacheData<UserCache>();
            await GetDatesCache(botClient, update);

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