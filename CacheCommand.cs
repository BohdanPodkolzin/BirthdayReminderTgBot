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
        public static Dictionary<string, DateTime> reminderDict = new Dictionary<string, DateTime>();
        public static void UpdateCache(string name, DateTime date)
        {
            if (!reminderDict.ContainsKey(name))
            {
                reminderDict.Add(name, date);
            }
            else
            {
                reminderDict[name] = date;
            }
        }


        public static async Task GetDatesCache(ITelegramBotClient botClient, Update update)
        {
            var cache = update.GetCacheData<UserCache>();

            UpdateCache(cache.PersonName, cache.DateT);


            string message = $"User`s name {cache.PersonName}, date {cache.DateT}";
            //Message _ = await PRTelegramBot.Helpers.Message.Send(botClient, update, message);

        }

        [ReplyMenuHandler("Show Countdown")]
        public static async Task CheckCache(ITelegramBotClient botClient, Update update)
        {
            await GetDatesCache(botClient, update);
            string message;
            if (reminderDict.Count > 0)
            {
                message = "Users in the cache:";
                foreach (var user in reminderDict)
                {
                    message += $"\n- Name: {user.Key}, Date: {user.Value}";
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