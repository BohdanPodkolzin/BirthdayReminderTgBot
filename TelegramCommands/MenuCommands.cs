﻿using BirthdayReminder.Enums;
using PRTelegramBot.Attributes;
using PRTelegramBot.Interface;
using PRTelegramBot.Models;
using PRTelegramBot.Models.InlineButtons;
using PRTelegramBot.Utils;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace BirthdayReminder.TelegramCommands
{

    public class MenuCommands
    {
        private static Dictionary<long, int> _editCountdownMessageIds = new Dictionary<long, int>();

        [ReplyMenuHandler("/menu")]
        public static async Task Menu(ITelegramBotClient botClient, Update update)
        {
            var menuMessage = $"Homepage🏡";

            List<KeyboardButton> menuList = new List<KeyboardButton>();
            menuList.Add(new KeyboardButton("About"));
            menuList.Add(new KeyboardButton("Edit Countdown"));
            menuList.Add(new KeyboardButton("Show countdown"));

            var menu = MenuGenerator.ReplyKeyboard(2, menuList);
            var option = new OptionMessage();
            option.MenuReplyKeyboardMarkup = menu;

            var showMenu = await PRTelegramBot.Helpers.Message.Send(botClient, update, menuMessage, option);

        }

        [ReplyMenuHandler("About")]
        public static async Task About(ITelegramBotClient botClient, Update update)
        {
            var message = $"This bot was created by @szymptom like first project.\nThis creation is aimed at helping with the schedule of the day of births";
            var _ = await PRTelegramBot.Helpers.Message.Send(botClient, update, message);
        }



        [ReplyMenuHandler("Edit Countdown")]
        public static async Task EditCountdown(ITelegramBotClient botClient, Update update)
        {
            var chatId = update.Message.Chat.Id;
            var prevMessageId = GetMessageId(chatId);

            if (prevMessageId != -1)
            {
                await botClient.DeleteMessageAsync(chatId, prevMessageId);
            }

            var addButton = new InlineCallback("Add Countdown", EditCountdownTHeader.Add);
            var delButton = new InlineCallback("Delete Countdown", EditCountdownTHeader.Del);
            var allDelButton = new InlineCallback("Delete All Schedule", EditCountdownTHeader.AllDel);

            List<IInlineContent> menu = new();
            menu.Add(addButton);
            menu.Add(delButton);
            menu.Add(allDelButton);

            var editorMenu = MenuGenerator.InlineKeyboard(2, menu);

            var option = new OptionMessage();
            option.MenuInlineKeyboardMarkup = editorMenu;

            var message = "Editing Schedule";
            var sendMessage = await PRTelegramBot.Helpers.Message.Send(botClient, update, message, option);
            
            SetMessageId(chatId, sendMessage.MessageId);
        }

        public static int GetMessageId(long chatId)
        {
            if (_editCountdownMessageIds.ContainsKey(chatId))
            {
                var messageId = _editCountdownMessageIds[chatId];
                _editCountdownMessageIds.Remove(chatId);
                return messageId;
            }
            return -1;
        }

        public static void SetMessageId(long chatId, int messageId)
        {
            _editCountdownMessageIds[chatId] = messageId;
        }

    }
}