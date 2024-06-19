using System;
using System.Collections.Generic;
using BirthdayReminder.PersonReminder;
using BirthdayReminder.Telegram.Helpers;
using PRTelegramBot.Attributes;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using TgMessageHelper = PRTelegramBot.Helpers.Message;

namespace BirthdayReminder.Telegram.CommandHandlers.StartBotCommand
{
    public class ConfirmingTimezoneHandler
    {
        public static async Task ConfirmingTimezoneMenu(ITelegramBotClient botClient, Update update, string message)
        {
            var option = ReplyKeyboardHelper.GetConfirmationTimezoneMenu().AsOption();
            await TgMessageHelper.Send(botClient, update, message, option);
        }
    }
}
