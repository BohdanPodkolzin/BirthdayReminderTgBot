﻿using PRTelegramBot.Attributes;
using PRTelegramBot.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using tg.UsersCache;

namespace tg.PersonReminder
{
    public class ReminderBack
    {

        public static async Task RemindPersonForBirthday(ITelegramBotClient botClient, Update update)
        {
            DateTime currDate = DateTime.Today;
            var cache = update.GetCacheData<UserCache>();

            foreach (var user in cache.scheduleDict)
            {
                if (user.Value.Month.Equals(currDate.Month) && user.Value.Day.Equals(currDate.Day))
                {
                    string message = $"Today is {user.Key}'s birthday!";
                    Message _ = await PRTelegramBot.Helpers.Message.Send(botClient, update, message);
                }
                else if (user.Value.Month.Equals(currDate.Month) && user.Value.Day.Equals(currDate.Day + 1))
                {
                    string message = $"{user.Key}'s birthday is tomorrow!";
                    Message _ = await PRTelegramBot.Helpers.Message.Send(botClient, update, message);
                }
            }
        }
    }
}
