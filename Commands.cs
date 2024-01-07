using PRTelegramBot.Attributes;
using PRTelegramBot.Core;
using PRTelegramBot.Models;
using PRTelegramBot.Utils;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace tg
{
    public class Commands
    {
        [ReplyMenuHandler("/start")]
        public static async Task StartBot(ITelegramBotClient botClient, Update update)
        {   
            var startMessage = $"Get ready, to do list of bithdays enter /menu";
            var printStartedMessage = await PRTelegramBot.Helpers.Message.Send(botClient, update, startMessage);
            // var firstMessage = $"Hello, {update.Message.From.FirstName}!\n This bot was created by @szymptom like first project. This creation is aimed at helping with the schedule of the day of births";
            // var printFirstMessage = await PRTelegramBot.Helpers.Message.Send(botClient, update, firstMessage);

        }


        [ReplyMenuHandler("/menu")]
        public static async Task Menu(ITelegramBotClient botClient, Update update)
        {   
            string menuMessage = $"All opportunities of this bot";
            
            List<KeyboardButton> menuList = new List<KeyboardButton>();
            menuList.Add(new KeyboardButton("About"));
            menuList.Add(new KeyboardButton("Edit Countdown"));

            ReplyKeyboardMarkup menu = MenuGenerator.ReplyKeyboard(2, menuList);
            OptionMessage option = new OptionMessage();
            option.MenuReplyKeyboardMarkup = menu;

            Message showMenu = await PRTelegramBot.Helpers.Message.Send(botClient, update, menuMessage, option);

        }
    }
}
