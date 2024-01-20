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

        [ReplyMenuHandler("step")]
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
            Message _ = await PRTelegramBot.Helpers.Message.Send(botClient, update, message);
            await Calendar.PickCalendar(botClient, update);
            //await Calendar.PickCalendar(botClient, update);
        }
        //public static async Task StepTwo(ITelegramBotClient botClient, Update update)
        //{
        //    string message = $"Entered date {update.Message?.Text}";

        //    var handler = update.GetStepHandler<StepTelegram>();
        //    handler!.GetCache<UserCache>().DateT = update.Message?.Text;
        //    handler.RegisterNextStep(StepThree, DateTime.Now.AddMinutes(1));

        //    Message _ = await PRTelegramBot.Helpers.Message.Send(botClient, update, message);
        //}

        //public static async Task StepThree(ITelegramBotClient botClient, Update update)
        //{
        //    var handler = update.GetStepHandler<StepTelegram>();
        //    var cache = update.GetCacheData<UserCache>();
        //    var cacheUser = handler!.GetCache<UserCache>();
        //    cache.DateT = cacheUser.DateT;
        //    cache.PersonName = cacheUser.PersonName;
        //    string message = $"Entered date {cacheUser.DateT} and name {cacheUser.PersonName}";

        //    update.ClearStepUserHandler();

        //    Message _ = await PRTelegramBot.Helpers.Message.Send(botClient, update, message);
        //}
    }
}