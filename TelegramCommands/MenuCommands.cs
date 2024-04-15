using PRTelegramBot.Attributes;
using PRTelegramBot.Models;
using PRTelegramBot.Models.InlineButtons;
using PRTelegramBot.Utils;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using System;
using PRTelegramBot.Interface;
using PRTelegramBot.Extensions;
using tg.Models;
using tg.UsersCache;
using tg.PersonReminder;

namespace tg.TelegramCommands
{
    public class MenuCommands
    {

        [ReplyMenuHandler("/menu")]
        public static async Task Menu(ITelegramBotClient botClient, Update update)
        {
            string menuMessage = $"Homepage🏡";

            List<KeyboardButton> menuList = new List<KeyboardButton>();
            menuList.Add(new KeyboardButton("About"));
            menuList.Add(new KeyboardButton("Edit Countdown"));
            menuList.Add(new KeyboardButton("Show countdown"));

            ReplyKeyboardMarkup menu = MenuGenerator.ReplyKeyboard(2, menuList);
            OptionMessage option = new OptionMessage();
            option.MenuReplyKeyboardMarkup = menu;

            Message showMenu = await PRTelegramBot.Helpers.Message.Send(botClient, update, menuMessage, option);

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
            var addButton = new InlineCallback("Add Countdown", EditCountdownTHeader.Add);
            var delButton = new InlineCallback("Delete Countdown", EditCountdownTHeader.Del);
            var allDelButton = new InlineCallback("Delete All Schedule", EditCountdownTHeader.AllDel);

            List<IInlineContent> menu = new();
            menu.Add(addButton);
            menu.Add(delButton);
            menu.Add(allDelButton);

            InlineKeyboardMarkup editorMenu = MenuGenerator.InlineKeyboard(2, menu);

            OptionMessage option = new OptionMessage();
            option.MenuInlineKeyboardMarkup = editorMenu;

            string message = "Editing Schedule";
            Message _ = await PRTelegramBot.Helpers.Message.Send(botClient, update, message, option);
        }

    }
}