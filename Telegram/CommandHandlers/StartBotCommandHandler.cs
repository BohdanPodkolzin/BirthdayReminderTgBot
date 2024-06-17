    using BirthdayReminder.DependencyInjectionConfiguration;
    using BirthdayReminder.PersonReminder;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json.Linq;
    using PRTelegramBot.Attributes;
    using PRTelegramBot.Extensions;
    using PRTelegramBot.Models;
    using Telegram.Bot;
    using Telegram.Bot.Types;

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

                var user = update.Message.From;
                var userNickName = user?.Username ?? "";

                var startMessage = $"🖐️ Hey, @{userNickName}!\n" +
                                   $"For the bot to work correctly, specify the city closest to you:";
                
                update.RegisterStepHandler(new StepTelegram(GetUserTimezone));
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

                var message = await GetTimezone(latitude, longitude);

                update.ClearStepUserHandler();
                await PRTelegramBot.Helpers.Message.Send(botClient, update, message);
            }
            
            private static async Task<(string? lat, string? lon)> GetLatitudeAndLongitude(string city)
            {
                var url = $"https://nominatim.openstreetmap.org/search?q={Uri.EscapeDataString(city)}&format=json";
                using HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("User-Agent", "CSharpApp");

                var response = await client.GetStringAsync(url);
                var results = JArray.Parse(response);

                var jsonResult = results[0];

                var lat = jsonResult["lat"]?.Value<string>();
                var lon = jsonResult["lon"]?.Value<string>();

                return (lat, lon);
            }

            private static async Task<string> GetTimezone(string lat, string lon)
            {
                var url =
                    $"https://api.timezonedb.com/v2.1/get-time-zone?key=7TVJUMUJ9LMG&format=json&by=position&lat={lat}&lng={lon}";
                using HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("User-Agent", "CSharpApp");

                var response = await client.GetStringAsync(url);
                var results = JObject.Parse(response);

                var town = results["cityName"]?.Value<string>();
                var region = results["countryName"]?.Value<string>();
                var time = results["formatted"]?.Value<string>();

                var message =
                    $"City: {town}, {region}\n" +
                    $"Time: {time}";

                return message;
            }
        }
    }
