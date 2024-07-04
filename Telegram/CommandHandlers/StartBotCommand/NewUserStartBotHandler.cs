using BirthdayReminder.PersonReminder;
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
    public class NewUserStartBotHandler
    {
        public static async Task NewUserStartBot(ITelegramBotClient botClient, Update update)
        {
            if (update.Message?.From == null) return;

            const string message = "For the bot to work correctly, specify the city closest to you:";

            update.RegisterStepHandler(new StepTelegram(GetUserTimezone, new RecordsCoordinatesModel()));

            await PRTelegramBot.Helpers.Message.Send(botClient, update, message);
            await InfinityLoop.StartReminderLoop(botClient, update);
        }

        public static async Task GetUserTimezone(ITelegramBotClient botClient, Update update)
        {
            var city = update.Message?.Text;
            if (city == null) return;

            var (latitude, longitude) = await GetLatitudeAndLongitudeFromApi(city);

            var message = await GetPlaceInformation(latitude, longitude);
            await ConfirmingTimezoneHandler.ConfirmingTimezoneMenu(botClient, update, message);

            var handler = update.GetStepHandler<StepTelegram>();
            handler!.GetCache<RecordsCoordinatesModel>().Latitude = latitude;
            handler!.GetCache<RecordsCoordinatesModel>().Longitude = longitude;
            handler!.RegisterNextStep(ConfirmingTimezone);
        }

        public static async Task ConfirmingTimezone(ITelegramBotClient botClient, Update update)
        {

            var handler = update.GetStepHandler<StepTelegram>();
            var cache = handler!.GetCache<RecordsCoordinatesModel>();
            if (update.Message?.From == null || cache.Latitude == null || cache.Longitude == null) return;

            var userId = update.Message.From.Id;
            var userChoice = update.Message?.Text;
            switch (userChoice)
            {
                case "Confirm":
                {
                    await IncludeLatitudeAndLongitude(userId, cache.Latitude, cache.Longitude);
                    await Menu(botClient, update);
                    
                    update.ClearStepUserHandler();
                    cache.ClearData();
                    break;
                }
                default:
                {
                    await PRTelegramBot.Helpers.Message.Send(botClient, update, "Please enter the city name");

                    handler!.RegisterNextStep(GetUserTimezone);
                    cache.ClearData();
                    break;
                }
            }
        }
    }
}
