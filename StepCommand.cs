using PRTelegramBot.Attributes;
using PRTelegramBot.Extensions;
using PRTelegramBot.Models;
using PRTelegramBot.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using StepTelegram = PRTelegramBot.Models.StepTelegram;

namespace tg
{
    public class StepCommand
    {
        [ReplyMenuHandler("step")]
        public static async Task StepStart(ITelegramBotClient botClient, Update update)
        {
            string msg = "Write person`s name";

            update.RegisterStepHandler(new StepTelegram(StepOne, new UserCacheTemporary()));
            Message _ = await PRTelegramBot.Helpers.Message.Send(botClient, update, msg);
        }

        public static async Task StepOne(ITelegramBotClient botClient, Update update)
        {
            string message = $"Testing: {update.Message?.Text}\nEnterbirthdayDate";

            var handler = update.GetStepHandler<StepTelegram>();
            handler!.GetCache<UserCacheTemporary>().PersonName = update.Message?.Text;

            handler.RegisterNextStep(StepTwo, DateTime.Now.AddMinutes(8));

            Message _ = await PRTelegramBot.Helpers.Message.Send(botClient, update, message);
        }

        public static async Task StepTwo(ITelegramBotClient botClient, Update update)
        {
            string message = $"TestingStepTwo: {update.Message?.Text}";
            var handler = update.GetStepHandler<StepTelegram>();
            handler!.GetCache<UserCacheTemporary>().Date = update.Message?.Text;

            Message _ = await PRTelegramBot.Helpers.Message.Send(botClient, update, message);
            update.ClearStepUserHandler();
        }
    }
}
