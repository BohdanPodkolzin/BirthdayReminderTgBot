using BirthdayReminder.Telegram.Helpers;
using PRTelegramBot.Attributes;
using PRTelegramBot.Extensions;
using PRTelegramBot.Models;
using PRTelegramBot.Utils;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace BirthdayReminder.Telegram.CommandHandlers
{
    public static class MenuCommands
    {
        private static readonly Dictionary<long, int> EditCountdownMessageIds = new();

        [ReplyMenuHandler("/menu")]
        public static async Task Menu(ITelegramBotClient botClient, Update update)
        {
            const string menuMessage = "Homepage🏡";

            List<KeyboardButton> menuList =
            [
                new KeyboardButton("About"),
                new KeyboardButton("Edit Countdown"),
                new KeyboardButton("Show countdown")
            ];

            var menu = MenuGenerator.ReplyKeyboard(2, menuList);
            var option = new OptionMessage
            {
                MenuReplyKeyboardMarkup = menu
            };

            await PRTelegramBot.Helpers.Message.Send(botClient, update, menuMessage, option);
        }

        [ReplyMenuHandler("About")]
        public static async Task About(ITelegramBotClient botClient, Update update)
        {
            const string message = "This bot was created by @szymptom like first project.\n" +
                                   "This creation is aimed at helping with the schedule of the day of births";
            await PRTelegramBot.Helpers.Message.Send(botClient, update, message);
        }

        [ReplyMenuHandler("Edit Countdown")]
        public static async Task EditCountdown(ITelegramBotClient botClient, Update update)
        {
            var chatId = update.GetChatId();

            var prevMessageId = GetPrevMessageIdInChat(chatId);
            if (prevMessageId != -1)
            {
                await botClient.DeleteMessageAsync(chatId, prevMessageId);
            }

            var editorMenu = InlineKeyboardsHelper.MenuKeyboard();
            var option = new OptionMessage
            {
                MenuInlineKeyboardMarkup = editorMenu
            };

            const string message = "Editing Schedule";
            var sendMessage = await PRTelegramBot.Helpers.Message.Send(botClient, update, message, option);
            
            SavePrevMessageIdInChat(chatId, sendMessage.MessageId);
        }

        public static int GetPrevMessageIdInChat(long chatId)
            => EditCountdownMessageIds.Remove(chatId, out var value) ? value : -1;

        public static void SavePrevMessageIdInChat(long chatId, int messageId)
            => EditCountdownMessageIds[chatId] = messageId;
    }
}
