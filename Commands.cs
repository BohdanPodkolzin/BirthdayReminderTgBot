using PRTelegramBot.Attributes;
using PRTelegramBot.Core;
using PRTelegramBot.Models;
using PRTelegramBot.Models.CallbackCommands;
using PRTelegramBot.Models.InlineButtons;
using PRTelegramBot.Models.Interface;
using PRTelegramBot.Utils;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using System;

namespace tg
{
    public class Commands
    {
        [SlashHandler("/start")]
        [ReplyMenuHandler("/start")]
        public static async Task StartBot(ITelegramBotClient botClient, Update update)
        {   
            var startMessage = $"Get ready, to do list of bithdays enter /menu";
            var printStartedMessage = await PRTelegramBot.Helpers.Message.Send(botClient, update, startMessage);
            // var firstMessage = $"Hello, {update.Message.From.FirstName}!\n This bot was created by @szymptom like first project. This creation is aimed at helping with the schedule of the day of births";
            // var printFirstMessage = await PRTelegramBot.Helpers.Message.Send(botClient, update, firstMessage);

        }

        [ReplyMenuHandler("About")]
        public static async Task About(ITelegramBotClient botClient, Update update)
        {
            var message = $"This bot was created by @szymptom like first project.\nThis creation is aimed at helping with the schedule of the day of births";
            var printMessage = await PRTelegramBot.Helpers.Message.Send(botClient, update, message);
        }



        [ReplyMenuHandler("/menu")]
        [SlashHandler("/menu")]
        public static async Task Menu(ITelegramBotClient botClient, Update update)
        {   
            string menuMessage = $"All opportunities of this bot";
            
            List<KeyboardButton> menuList = new List<KeyboardButton>();
            menuList.Add(new KeyboardButton("About"));
            menuList.Add(new KeyboardButton("Edit Countdown"));
            menuList.Add(new KeyboardButton("Show countdown"));

            ReplyKeyboardMarkup menu = MenuGenerator.ReplyKeyboard(2, menuList);
            OptionMessage option = new OptionMessage();
            option.MenuReplyKeyboardMarkup = menu;

            Message showMenu = await PRTelegramBot.Helpers.Message.Send(botClient, update, menuMessage, option);

        }

        [ReplyMenuHandler("Edit Countdown")]
        public static async Task EditCountdown(ITelegramBotClient botClient, Update update)
        {
            var message = $"Choose what you want to do";

            List<IInlineContent> menu = new List<IInlineContent>();

            var url = new InlineURL("Add", "https://google.com");
            menu.Add(url);


            InlineKeyboardMarkup menuItems = MenuGenerator.InlineKeyboard(1, menu);
            var optionss = new OptionMessage();
            optionss.MenuInlineKeyboardMarkup = menuItems;

            var senddMessage = await PRTelegramBot.Helpers.Message.Send(botClient, update, message, optionss);
        }
    }
}
