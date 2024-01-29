using PRTelegramBot.Attributes;
using PRTelegramBot.Extensions;
using PRTelegramBot.Models;
using PRTelegramBot.Models.CallbackCommands;
using PRTelegramBot.Models.Enums;
using PRTelegramBot.Models.InlineButtons;
using PRTelegramBot.Utils.Controls.CalendarControl.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace tg
{
    public class StepCommands
    {

        public static async Task StartStep(ITelegramBotClient botClient, Update update)
        {
            string message = "Write persona name";
            update.RegisterStepHandler(new StepTelegram(StepOne, new UserCache()));
            Message _ = await PRTelegramBot.Helpers.Message.Send(botClient, update, message);
        }

        public static async Task StepOne(ITelegramBotClient botClient, Update update)
        {
            string message = $"Entered name {update.Message?.Text}\nEnter Birth date";

            var cache = update.GetCacheData<UserCache>();
            cache.PersonName = update.Message?.Text;

            update.ClearStepUserHandler();
            await Calendar.PickCalendar(botClient, update);
        }
    }
}