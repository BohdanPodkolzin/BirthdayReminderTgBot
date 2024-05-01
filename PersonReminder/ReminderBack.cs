﻿using BirthdayReminder.Telegram.Models;
using PRTelegramBot.Extensions;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BirthdayReminder.PersonReminder
{
    public static class ReminderBack
    {
        public static async Task RemindPersonForBirthday(ITelegramBotClient botClient, Update update)
        {
            var currDate = DateTime.Today;
            var cache = update.GetCacheData<UserCache>();

            foreach (var user in cache.ScheduleDict)
            {
                if (user.Value.Month.Equals(currDate.Month) && user.Value.Day.Equals(currDate.Day))
                {
                    var message = $"Today is {user.Key}'s birthday!";
                    _ = await PRTelegramBot.Helpers.Message.Send(botClient, update, message);
                }
                else if (user.Value.Month.Equals(currDate.Month) && user.Value.Day.Equals(currDate.Day + 1))
                {
                    var message = $"{user.Key}'s birthday is tomorrow!";
                    _ = await PRTelegramBot.Helpers.Message.Send(botClient, update, message);
                }
            }
        }
    }
}
