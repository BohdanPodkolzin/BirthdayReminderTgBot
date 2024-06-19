using BirthdayReminder.PersonReminder;
using BirthdayReminder.Telegram.Models;
using PRTelegramBot.Attributes;
using PRTelegramBot.Extensions;
using PRTelegramBot.Models;
using Telegram.Bot;
using Telegram.Bot.Types;
using static BirthdayReminder.Telegram.Helpers.StartBotHelper;

namespace BirthdayReminder.Telegram.CommandHandlers
{
    public class StartBotCommandHandler
    {

        [ReplyMenuHandler("/start")]
        public static async Task StartBotMethod(ITelegramBotClient botClient, Update update)
        {
            if (update.Message?.From == null)
            {
                return;
            }

            var userTelegramTag = update.Message.From?.Username ?? "";
            var startMessage = $"🖐️ Hey, @{userTelegramTag}!\n" +
                                $"For the bot to work correctly, specify the city closest to you:";
                
            update.RegisterStepHandler(new StepTelegram(GetUserTimezone, new PlaceCoordinatesCache()));

            await PRTelegramBot.Helpers.Message.Send(botClient, update, startMessage);
            await InfinityLoop.StartReminderLoop(botClient, update);
        }

        public static async Task GetUserTimezone(ITelegramBotClient botClient, Update update)
        {
            var city = update.Message?.Text;
            if (city == null)
            {
                await PRTelegramBot.Helpers.Message.Send(botClient, update, "Please enter the city name");
                return;
            }

            var (latitude, longitude) = await GetLatitudeAndLongitude(city);
            if (latitude == null || longitude == null)
            {
                await PRTelegramBot.Helpers.Message.Send(botClient, update, "City not found");
                return;
            }
            
            //var cache = update.GetCacheData<PlaceCoordinatesCache>();
            //if (update.Message?.From == null)
            //{
            //    return;
            //}

            //cache.UserId = update.Message.From.Id;
            //cache.Latitude = latitude;
            //cache.Longitude = longitude;

            var message = await GetPlaceInformation(latitude, longitude);
            await ConfirmingTimezoneHandler.ConfirmingTimezoneMenu(botClient, update, message);
            
            update.ClearStepUserHandler();
        }
    }
}
