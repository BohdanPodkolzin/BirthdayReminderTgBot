﻿using PRTelegramBot.Attributes;
using PRTelegramBot.Extensions;
using PRTelegramBot.Interface;
using PRTelegramBot.Models.InlineButtons;
using PRTelegramBot.Models;
using PRTelegramBot.Utils;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot;
using Telegram.Bot.Types;
using tg.Models;
using tg.UsersCache;

namespace tg.TelegramCommands
{
    public class EditCountdownCommands
    {


        [InlineCallbackHandler<EditCountdownTHeader>(EditCountdownTHeader.Add)]
        public static async Task AddStepOne(ITelegramBotClient botClient, Update update)
        {
            try
            {
                var command = InlineCallback.GetCommandByCallbackOrNull(update.CallbackQuery?.Data ?? "");
                if (command != null)
                {
                    string message = $"Enter the name of the person";
                    Message _ = await PRTelegramBot.Helpers.Message.Send(botClient, update, message);
                    update.RegisterStepHandler(new StepTelegram(AddStepTwo, new UserCache()));

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public static async Task AddStepTwo(ITelegramBotClient botClient, Update update)
        {
            string message = $"Entered name <b>{update.Message?.Text}</b>";
            Message _ = await PRTelegramBot.Helpers.Message.Send(botClient, update, message);

            await Calendar.PickCalendar(botClient, update);

            var cache = update.GetCacheData<UserCache>();
            cache.PersonName = update.Message?.Text;
        }


        [InlineCallbackHandler<EditCountdownTHeader>(EditCountdownTHeader.Del)]
        public static async Task StepOneDel(ITelegramBotClient botClient, Update update)
        {
            try
            {
                var command = InlineCallback.GetCommandByCallbackOrNull(update.CallbackQuery?.Data ?? "");
                if (command != null)
                {
                    string message = "Specify the name you want to remove from the list:";
                    Message _ = await PRTelegramBot.Helpers.Message.Send(botClient, update, message);
                    update.RegisterStepHandler(new StepTelegram(StepTwoDate, new UserCache()));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public static async Task StepTwoDate(ITelegramBotClient botClient, Update update)
        {
            string? enteredName = update.Message?.Text;
            string message = $"There is no person with name {enteredName}\nPlease enter a valid title";

            var cache = update.GetCacheData<UserCache>();
            foreach (string userName in cache.scheduleDict.Keys)
            {
                if (userName.Equals(enteredName))
                {
                    cache.scheduleDict.Remove(userName);
                    message = $"<b>{enteredName}</b> is no longer in the schedule";
                }
            }

            Message _ = await PRTelegramBot.Helpers.Message.Send(botClient, update, message);
        }


        [InlineCallbackHandler<EditCountdownTHeader>(EditCountdownTHeader.AllDel)]
        public static async Task Confirm(ITelegramBotClient botClient, Update update)
        {
            int prevMessage = MenuCommands.GetMessageId();
            botClient.DeleteMessageAsync(update.CallbackQuery.Message.Chat.Id, prevMessage);

            var message = "Confirm clearing all list";

            var yesButton = new InlineCallback("Yes", ConfirmationTHeader.Yes);
            var noButton = new InlineCallback("No", ConfirmationTHeader.No);

            List<IInlineContent> yesOrNo = new List<IInlineContent>();
            yesOrNo.Add(yesButton);
            yesOrNo.Add(noButton);

            InlineKeyboardMarkup inlineKeyboard = MenuGenerator.InlineKeyboard(2, yesOrNo);

            OptionMessage option = new OptionMessage();
            option.MenuInlineKeyboardMarkup = inlineKeyboard;
            
            await PRTelegramBot.Helpers.Message.Send(botClient, update, message, option);
        }
    }
}