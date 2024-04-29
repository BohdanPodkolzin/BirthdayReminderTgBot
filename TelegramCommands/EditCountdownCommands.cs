using BirthdayReminder.Enums;
using BirthdayReminder.UsersCache;
using PRTelegramBot.Attributes;
using PRTelegramBot.Extensions;
using PRTelegramBot.Interface;
using PRTelegramBot.Models;
using PRTelegramBot.Models.InlineButtons;
using PRTelegramBot.Utils;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BirthdayReminder.TelegramCommands
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

            await Calendar.Calendar.PickCalendar(botClient, update);

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

        [InlineCallbackHandler<EditCountdownTHeader>(EditCountdownTHeader.AllDel)]
        public static async Task Confirm(ITelegramBotClient botClient, Update update)
        {
            var chatId = update.GetChatId();;
            var prevMessageId = MenuCommands.GetPrevMessageIdInChat(chatId);
            var message = "Confirm removing all Countdowns";

            var yesButton = new InlineCallback("Yes", ConfirmationTHeader.Yes);
            var noButton = new InlineCallback("No", ConfirmationTHeader.No);

            List<IInlineContent> yesOrNo = new List<IInlineContent>();
            yesOrNo.Add(yesButton);
            yesOrNo.Add(noButton);
            var inlineKeyboard = MenuGenerator.InlineKeyboard(2, yesOrNo);



            var sentMessage = await botClient.EditMessageTextAsync(chatId, prevMessageId, message, replyMarkup: inlineKeyboard);
            MenuCommands.SavePrevMessageIdInChat(chatId, sentMessage.MessageId);

        }



        [InlineCallbackHandler<ConfirmationTHeader>(ConfirmationTHeader.Yes)]
        public static async Task ClearCache(ITelegramBotClient botClient, Update update)
        {
            var message = "Countdowns has been successfully removed!";
            update.GetCacheData<UserCache>().ClearData();

            var chatId = update.GetChatId();
            var messageId = update.CallbackQuery.Message.MessageId;

            await botClient.EditMessageTextAsync(
                chatId,
                messageId,
                text: message
            );
        }

        [InlineCallbackHandler<ConfirmationTHeader>(ConfirmationTHeader.No)]
        public static async Task BackToEditCountdown(ITelegramBotClient botClient, Update update)
        {
            var chatId = update.GetChatId();
            var messageId = update.CallbackQuery.Message.MessageId;
            var message = "Editing Schedule";
            


            var addButton = new InlineCallback("Add Countdown", EditCountdownTHeader.Add);
            var delButton = new InlineCallback("Delete Countdown", EditCountdownTHeader.Del);
            var allDelButton = new InlineCallback("Delete All Schedule", EditCountdownTHeader.AllDel);

            List<IInlineContent> menu = new();
            menu.Add(addButton);
            menu.Add(delButton);
            menu.Add(allDelButton);

            var editorMenu = MenuGenerator.InlineKeyboard(2, menu);

            await botClient.EditMessageTextAsync(chatId, messageId, message, replyMarkup: editorMenu);
        }

    }
    
}       
