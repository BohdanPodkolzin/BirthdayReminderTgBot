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
using tg.Models;
using tg.UsersCache;
using tg.PersonReminder;

namespace tg.TelegramCommands
{
    public class Commands
    {
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
                    await InfinityLoop.StartReminderLoop(botClient, update);
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
        public static async Task AddStepOne(ITelegramBotClient botClient, Update update)
        {
            try
            {
                var command = InlineCallback.GetCommandByCallbackOrNull(update.CallbackQuery?.Data ?? "");
                if (command != null)
                {
                    string message = $"Enter person`s Name";
                    Message _ = await PRTelegramBot.Helpers.Message.Send(botClient, update, message);
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
            string message = $"Entered name {update.Message?.Text}";
            Message _ = await PRTelegramBot.Helpers.Message.Send(botClient, update, message);

            await Calendar.PickCalendar(botClient, update);

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
                    string message = "Specify the name you want to remove from the list:";
                    Message _ = await PRTelegramBot.Helpers.Message.Send(botClient, update, message);
                    update.RegisterStepHandler(new StepTelegram(StepTwoDate, new  UserCache()));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        
        public static async Task StepTwoDate(ITelegramBotClient botClient, Update update)
        {
            string? enteredName = update.Message?.Text;
            string message = $"There is no person with name {enteredName}\nPlease enter a valid title";

            var cache = update.GetCacheData<UserCache>();
            foreach (string userName in cache.scheduleDict.Keys)
            {
                if (userName.Equals(enteredName))
                {
                    cache.scheduleDict.Remove(userName);
                    message = $"<b>{enteredName}</b> is no longer in the schedule";
                }
            }

            Message _ = await PRTelegramBot.Helpers.Message.Send(botClient, update, message);
        }





    }
}