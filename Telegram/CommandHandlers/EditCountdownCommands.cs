﻿using BirthdayReminder.Telegram.Helpers;
using BirthdayReminder.Telegram.InlineCommands;
using BirthdayReminder.Telegram.Models;
using PRTelegramBot.Attributes;
using PRTelegramBot.Extensions;
using PRTelegramBot.Models;
using PRTelegramBot.Models.InlineButtons;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BirthdayReminder.Telegram.CommandHandlers
{
    public class EditCountdownCommands
    {
        [InlineCallbackHandler<CountdownInlineCommandTHeader>(CountdownInlineCommandTHeader.Add)]
        public static async Task AddStepOne(ITelegramBotClient botClient, Update update)
        {
            try
            {
                var command = InlineCallback.GetCommandByCallbackOrNull(update.CallbackQuery?.Data ?? "");
                if (command != null)
                {
                    var message = $"Enter the name of the person";
                    _ = await PRTelegramBot.Helpers.Message.Send(botClient, update, message);
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
            var message = $"Entered name <b>{update.Message?.Text}</b>";
            _ = await PRTelegramBot.Helpers.Message.Send(botClient, update, message);

            await CalendarCommandHandlers.PickCalendar(botClient, update);

            var cache = update.GetCacheData<UserCache>();
            cache.PersonName = update.Message?.Text;
        }


        [InlineCallbackHandler<CountdownInlineCommandTHeader>(CountdownInlineCommandTHeader.Del)]
        public static async Task StepOneDel(ITelegramBotClient botClient, Update update)
        {
            try
            {
                var command = InlineCallback.GetCommandByCallbackOrNull(update.CallbackQuery?.Data ?? "");
                if (command != null)
                {
                    var message = "Specify the name you want to remove from the list:";
                    _ = await PRTelegramBot.Helpers.Message.Send(botClient, update, message);
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
            var enteredName = update.Message?.Text;
            var message = $"There is no person with name {enteredName}\nPlease enter a valid title";

            var cache = update.GetCacheData<UserCache>();
            foreach (var userName in cache.ScheduleDict.Keys)
            {
                if (userName.Equals(enteredName))
                {
                    cache.ScheduleDict.Remove(userName);
                    message = $"<b>{enteredName}</b> is no longer in the schedule";
                }
            }

            _ = await PRTelegramBot.Helpers.Message.Send(botClient, update, message);
        }

        [InlineCallbackHandler<CountdownInlineCommandTHeader>(CountdownInlineCommandTHeader.AllDel)]
        public static async Task Confirm(ITelegramBotClient botClient, Update update)
        {
            const string message = "Confirm removing all Countdowns";

            var chatId = update.GetChatId();;
            var prevMessageId = MenuCommandHandlers.GetPrevMessageIdInChat(chatId);

            var inlineKeyboard = InlineKeyboardsHelper.ConfirmationKeyboard();
            var sentMessage = await botClient.EditMessageTextAsync(
                chatId, prevMessageId, message, replyMarkup: inlineKeyboard);

            MenuCommandHandlers.SavePrevMessageIdInChat(chatId, sentMessage.MessageId);
        }



        [InlineCallbackHandler<ConfirmationInlineCommandTHeader>(ConfirmationInlineCommandTHeader.Yes)]
        public static async Task ClearCache(ITelegramBotClient botClient, Update update)
        {
            const string message = "Countdowns has been successfully removed!";
            update.GetCacheData<UserCache>().ClearData();

            var chatId = update.GetChatId();
            var messageId = update.GetMessageId();

            await botClient.EditMessageTextAsync(
                chatId,
                messageId,
                text: message
            );
        }

        [InlineCallbackHandler<ConfirmationInlineCommandTHeader>(ConfirmationInlineCommandTHeader.No)]
        public static async Task BackToEditCountdown(ITelegramBotClient botClient, Update update)
        {
            const string message = "Editing Schedule";

            var chatId = update.GetChatId();
            var messageId = update.GetMessageId();

            var editorMenu = InlineKeyboardsHelper.CountdownMenuKeyboard();

            await botClient.EditMessageTextAsync(chatId, messageId, message, replyMarkup: editorMenu);
        }
    }
}       