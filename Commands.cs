using PRTelegramBot.Attributes;
using PRTelegramBot.Core;
using PRTelegramBot.Models;
using PRTelegramBot.Models.CallbackCommands;
using PRTelegramBot.Models.InlineButtons;

using PRTelegramBot.Utils;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using System;
using PRTelegramBot.Interface;
using PRTelegramBot.Exceptions;
using PRTelegramBot.Extensions;

namespace tg
{
    public class Commands
    {
        [SlashHandler("/start")]
        [ReplyMenuHandler("/start")]
        public static async Task StartBot(ITelegramBotClient botClient, Update update)
        {
            if (update.Message?.From != null)
            {
                User user = update.Message.From;
                string userNickName = user?.Username ?? "";

                if (userNickName != null)
                {
                    string startMessage = $"🖐️ Hey, @{userNickName}!\nTo make your first schedule of bithdays enter /menu";
                    Message _ = await PRTelegramBot.Helpers.Message.Send(botClient, update, startMessage);
                }
            }

        }

        [ReplyMenuHandler("About")]
        public static async Task About(ITelegramBotClient botClient, Update update)
        {
            var message = $"This bot was created by @szymptom like first project.\nThis creation is aimed at helping with the schedule of the day of births";
            var _ = await PRTelegramBot.Helpers.Message.Send(botClient, update, message);
        }



        [ReplyMenuHandler("/menu")]
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

            string message = "What do you want to do?";
            Message _ = await PRTelegramBot.Helpers.Message.Send(botClient, update, message, option);
        }


        [InlineCallbackHandler<EditCountdownTHeader>(EditCountdownTHeader.Add)]
        public static async Task Inline(ITelegramBotClient botClient, Update update)
        {
            try
            { 
                var command = InlineCallback.GetCommandByCallbackOrNull(update.CallbackQuery?.Data ?? "");
                if (command != null)
                {
                    string message = $"/addReminder";
                    Message _ = await PRTelegramBot.Helpers.Message.Send(botClient, update, message);
                    update.RegisterStepHandler(new StepTelegram(InlineSteps, new UserCache()));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        public static async Task InlineSteps(ITelegramBotClient botClient, Update update)
        {
            string message = $"User entered {update.Message?.Text}";
            
            Message _ = await PRTelegramBot.Helpers.Message.Send(botClient, update, message);


        }

        [InlineCallbackHandler<EditCountdownTHeader>(EditCountdownTHeader.AllDel)]
        public static async Task GetListClear(ITelegramBotClient botClient, Update update)
        {
            var command = InlineCallback.GetCommandByCallbackOrNull(update.CallbackQuery?.Data ?? "");
            if (command != null)
            {
                string message = $"Callback command AllDel";
                Message _ = await PRTelegramBot.Helpers.Message.Send(botClient, update, message);
            }
        }
    }
}