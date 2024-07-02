using BirthdayReminder.Telegram.CommandHandlers.StartBotCommand;
using BirthdayReminder.Telegram.Helpers;
using PRTelegramBot.Attributes;
using PRTelegramBot.Extensions;
using Telegram.Bot;
using Telegram.Bot.Types;
using TgMessageHelper = PRTelegramBot.Helpers.Message;

namespace BirthdayReminder.Telegram.CommandHandlers.MainMenuCommand
{
    public static class MenuCommandHandlers
    {
        private static readonly Dictionary<long, int> EditCountdownMessageIds = new();

        [ReplyMenuHandler("/menu")]
        public static async Task Menu(ITelegramBotClient botClient, Update update)
        {
            const string menuMessage = "Homepage🏡";
            var option = ReplyKeyboardHelper.GetBotMenu().AsOption();
            await TgMessageHelper.Send(botClient, update, menuMessage, option);
        }

        [ReplyMenuHandler("About")]
        public static async Task About(ITelegramBotClient botClient, Update update)
        {
            const string message = "This bot was created by @szymptom like first project.\n" +
                                   "This creation is aimed at helping with the schedule of the day of births";
            await TgMessageHelper.Send(botClient, update, message);
        }

        [ReplyMenuHandler("Edit Countdown")]
        public static async Task EditCountdown(ITelegramBotClient botClient, Update update)
        {
            var chatId = update.GetChatId();
            await RemoveBotDuplicateMessage(botClient, chatId);

            const string message = "Editing Schedule";
            var option = InlineKeyboardsHelper.CountdownMenuKeyboard().AsOption();
            var sendMessage = await TgMessageHelper.Send(botClient, update, message, option);

            SavePrevMessageIdInChat(chatId, sendMessage.MessageId);
        }

        private static async Task RemoveBotDuplicateMessage(ITelegramBotClient botClient, long chatId)
        {
            var prevMessageId = GetPrevMessageIdInChat(chatId);
            if (prevMessageId != -1)
            {
                await botClient.DeleteMessageAsync(chatId, prevMessageId);
            }
        }

        [ReplyMenuHandler("Edit Timezone")]
        public static async Task EditTimezone(ITelegramBotClient botClient, Update update) 
            => await NewUserStartBotHandler.NewUserStartBot(botClient, update);

        public static int GetPrevMessageIdInChat(long chatId)
            => EditCountdownMessageIds.Remove(chatId, out var value) ? value : -1;

        public static void SavePrevMessageIdInChat(long chatId, int messageId)
            => EditCountdownMessageIds[chatId] = messageId;
    }
}
