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
            //private static readonly ILogger<StartBotCommandHandler> _logger;

            //static StartBotCommandHandler()
            //{
            //    var serviceProvider = BotConfiguration.ServiceProvider;
            //    _logger = serviceProvider.GetRequiredService<ILogger<StartBotCommandHandler>>();

            //}

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
                                   $"To make your first schedule of birthdays enter /menu";
                
                update.RegisterStepHandler(new StepTelegram(GetUserTimezone));
                await PRTelegramBot.Helpers.Message.Send(botClient, update, startMessage);
                await InfinityLoop.StartReminderLoop(botClient, update);
            }

            public static async Task GetUserTimezone(ITelegramBotClient botClient, Update update)
            {
                var city = update.Message?.Text;

                var url = $"https://nominatim.openstreetmap.org/search?q={Uri.EscapeDataString(city)}&format=json";
                using HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("User-Agent", "CSharpApp");

                var response = await client.GetStringAsync(url);
                var results = JArray.Parse(response);

                if (results.Count == 0)
                {
                    await PRTelegramBot.Helpers.Message.Send(botClient, update, "City not found. Please enter the correct city name.");
                    return;
                }

                var jsonResult = results[0];

                var lat = jsonResult["lat"]?.Value<string>();
                var lon = jsonResult["lon"]?.Value<string>();

                var timeUrl =
                    $"https://api.timezonedb.com/v2.1/get-time-zone?key=7TVJUMUJ9LMG&format=json&by=position&lat={lat}&lng={lon}";
                var timeResponse = await client.GetStringAsync(timeUrl);
                var timeResults = JObject.Parse(timeResponse);

                var town = timeResults["cityName"]?.Value<string>();
                var region = timeResults["countryName"]?.Value<string>();
                var time = timeResults["formatted"]?.Value<string>();

                var message = 
                              $"City: {town}, {region}\n" +
                              $"Time: {time}";  

                update.ClearStepUserHandler();
                await PRTelegramBot.Helpers.Message.Send(botClient, update, message);
            }


        }
    }
