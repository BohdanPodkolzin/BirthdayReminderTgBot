using BirthdayReminder.Telegram.Models;
using PRTelegramBot.Extensions;
using PRTelegramBot.Models;
using Telegram.Bot;
using Telegram.Bot.Types;
using static BirthdayReminder.Telegram.Helpers.StartBotHelper;
using static BirthdayReminder.MySqlDataBase.DataBaseConnector.Queries;
using static BirthdayReminder.Telegram.CommandHandlers.MainMenuCommand.MenuCommandHandlers;

namespace BirthdayReminder.Telegram.CommandHandlers.StartBotCommand
{
    public class ExistingUserStartBotHandler
    {
        public static async Task ExistingUserStartBotMenu(ITelegramBotClient botClient, Update update)
        {
            if (update.Message?.From == null) return;
            
            var (latitude, longitude) = await GetLatitudeAndLongitudeFromDatabase(update.Message.From.Id);

            var message = await GetPlaceInformation(latitude, longitude);
            await PRTelegramBot.Helpers.Message.Send(botClient, update, message);

            await Menu(botClient, update);
        }
    }
}
