using BirthdayReminder.PersonReminder;
using BirthdayReminder.Telegram.Helpers;
using BirthdayReminder.Telegram.InlineCommands;
using BirthdayReminder.Telegram.Models;
using PRTelegramBot.Attributes;
using PRTelegramBot.Extensions;
using PRTelegramBot.Models;
using PRTelegramBot.Models.InlineButtons;
using Telegram.Bot;
using Telegram.Bot.Types;
using static BirthdayReminder.DataBase.DataBaseConnector.MySqlConnector;

namespace BirthdayReminder.Telegram.CommandHandlers
{
    public class EditCountdownCommands
    {
        private static UserCache GetUserCache(Update update) 
            => update.GetCacheData<UserCache>();

        [InlineCallbackHandler<CountdownInlineCommandTHeader>(CountdownInlineCommandTHeader.Add)]
        public static async Task CreateEventStepOne(ITelegramBotClient botClient, Update update)
        {
            try
            {
                var command = InlineCallback.GetCommandByCallbackOrNull(update.CallbackQuery?.Data ?? "");
                if (command is not { } __)
                {
                    return;
                }

                const string message = "Enter the name of the person";
                await PRTelegramBot.Helpers.Message.Edit(botClient, update, message);

                update.RegisterStepHandler(new StepTelegram(CreateEventStepTwo, GetUserCache(update)));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private static async Task CreateEventStepTwo(ITelegramBotClient botClient, Update update)
        {
            var personName = update.Message?.Text;
            
            if (personName != null)
            {
                GetUserCache(update).PersonName = personName;

                var message = $"Entered name <b>{update.Message?.Text}</b>";
                await PRTelegramBot.Helpers.Message.Send(botClient, update, message);
                await CalendarCommandHandlers.PickCalendar(botClient, update);
            }

        }


        [InlineCallbackHandler<CountdownInlineCommandTHeader>(CountdownInlineCommandTHeader.Del)]
        public static async Task DeleteEventStepOne(ITelegramBotClient botClient, Update update)
        {
            try
            {
                var command = InlineCallback.GetCommandByCallbackOrNull(update.CallbackQuery?.Data ?? "");
                if (command is not { } __)
                {
                    return;
                }

                string message;

                if (GetUserCache(update).ScheduleDict.Count <= 0)
                {
                    message = "There are no people in the schedule";
                    await PRTelegramBot.Helpers.Message.Send(botClient, update, message);
                    return;
                }

                message = "Specify the name you want to remove from the list:";
                await PRTelegramBot.Helpers.Message.Edit(botClient, update, message);

                update.RegisterStepHandler(new StepTelegram(DeleteEventStepTwo, GetUserCache(update)));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private static async Task DeleteEventStepTwo(ITelegramBotClient botClient, Update update)
        {
            var enteredName = update.Message?.Text;
            var message = $"There is no person with name {enteredName}\nPlease enter a valid title";

            var cache = GetUserCache(update);
            foreach (var userName in cache.ScheduleDict.Keys
                         .Where(userName => userName.Equals(enteredName)))
            {
                cache.ScheduleDict.Remove(userName);
                message = $"<b>{enteredName}</b> is no longer in the schedule";
            }

            await PRTelegramBot.Helpers.Message.Send(botClient, update, message);
        }

        [InlineCallbackHandler<CountdownInlineCommandTHeader>(CountdownInlineCommandTHeader.AllDel)]
        public static async Task Confirm(ITelegramBotClient botClient, Update update)
        {
            const string message = "Confirm removing all Countdowns";

            var chatId = update.GetChatId();
            var prevMessageId = MenuCommandHandlers.GetPrevMessageIdInChat(chatId);

            var inlineKeyboard = InlineKeyboardsHelper.ConfirmationKeyboard();
            var sentMessage = await botClient.EditMessageTextAsync(
                chatId, prevMessageId, message, replyMarkup: inlineKeyboard);

            MenuCommandHandlers.SavePrevMessageIdInChat(chatId, sentMessage.MessageId);
        }

        [InlineCallbackHandler<ConfirmationInlineCommandTHeader>(ConfirmationInlineCommandTHeader.Yes)]
        public static async Task ResetAllEvents(ITelegramBotClient botClient, Update update)
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
