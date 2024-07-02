using BirthdayReminder.Telegram.CommandHandlers.CalendarCommand;
using BirthdayReminder.Telegram.Helpers;
using BirthdayReminder.Telegram.InlineCommands;
using BirthdayReminder.Telegram.Models;
using PRTelegramBot.Attributes;
using PRTelegramBot.Extensions;
using PRTelegramBot.Models;
using PRTelegramBot.Models.InlineButtons;
using Telegram.Bot;
using Telegram.Bot.Types;
using static BirthdayReminder.MySqlDataBase.DataBaseConnector.Queries;

namespace BirthdayReminder.Telegram.CommandHandlers.MainMenuCommand
{
    public class EditCommandsHandler
    {
        private static RecordCache GetUserCache(Update update)
            => update.GetCacheData<RecordCache>();

        [InlineCallbackHandler<CountdownInlineCommandTHeader>(CountdownInlineCommandTHeader.Add)]
        public static async Task CreateEventStepOne(ITelegramBotClient botClient, Update update)
        {
            try
            {
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
                if (update.CallbackQuery?.From == null)
                {
                    return;
                }

                string message;
                if (await IsUserScheduleEmpty(update.CallbackQuery.From.Id))
                {
                    message = "<b>Your schedule is empty</b>";
                    await PRTelegramBot.Helpers.Message.Edit(botClient, update, message);
                    return;
                }

                message = "Specify the name you want to remove from the list:";
                await PRTelegramBot.Helpers.Message.Edit(botClient, update, message);

                update.RegisterStepHandler(new StepTelegram(DeleteEventStepTwo));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private static async Task DeleteEventStepTwo(ITelegramBotClient botClient, Update update)
        {
            var userInputName = update.Message?.Text;
            var message = $"There is no person with name {userInputName}\nPlease enter a valid title";

            if (update.Message?.From == null)
            {
                return;
            }

            if (await IsRecordExist(update.Message.From.Id, userInputName))
            {
                await RemoveRecordByName(update.Message.From.Id, userInputName);
                message = $"<b>{userInputName}</b> is no longer in the schedule";
            }

            await PRTelegramBot.Helpers.Message.Send(botClient, update, message);
            update.ClearStepUserHandler();
        }

        [InlineCallbackHandler<CountdownInlineCommandTHeader>(CountdownInlineCommandTHeader.AllDel)]
        public static async Task ConfirmResettingAllEvents(ITelegramBotClient botClient, Update update)
        {
            const string message = "Confirm removing all Countdowns";

            var chatId = update.GetChatId();
            var prevMessageId = MenuCommandHandlers.GetPrevMessageIdInChat(chatId);

            var inlineKeyboard = InlineKeyboardsHelper.ConfirmationKeyboard();
            var sentMessage = await botClient.EditMessageTextAsync(
                chatId,
                prevMessageId,
                message,
                replyMarkup: inlineKeyboard
                );

            MenuCommandHandlers.SavePrevMessageIdInChat(chatId, sentMessage.MessageId);
        }

        [InlineCallbackHandler<ConfirmationInlineCommandTHeader>(ConfirmationInlineCommandTHeader.Yes)]
        public static async Task ResetAllEvents(ITelegramBotClient botClient, Update update)
        {
            if (update.CallbackQuery?.Message?.From == null)
            {
                return;
            }

            await RemoveAllRecords(update.CallbackQuery.From.Id);

            const string message = "Countdowns has been successfully removed!";

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
