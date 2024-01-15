using PRTelegramBot.Attributes;
using PRTelegramBot.Extensions;
using PRTelegramBot.Models;
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

            update.RegisterStepHandler(new StepTelegram(StepOne, new UserCache()));
            Message _ = await PRTelegramBot.Helpers.Message.Send(botClient, update, msg);
        }

        public static async Task StepOne(ITelegramBotClient botClient, Update update)
        {

        }
    }
}
