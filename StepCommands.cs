using PRTelegramBot.Attributes;
using PRTelegramBot.Extensions;
using PRTelegramBot.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace tg
{
    public class StepCommands
    {
        [ReplyMenuHandler("step")]
        public static async Task StartStep(ITelegramBotClient botClient, Update update)
        {
            string message = "Write persona name";
            update.RegisterStepHandler(new StepTelegram(StepOne, new UserCache()));

            Message _ = await PRTelegramBot.Helpers.Message.Send(botClient, update, message);
        }

        public static async Task StepOne(ITelegramBotClient botClient, Update update)
        {
            string message = $"Entered name {update.Message?.Text}.\nEnter Birthday";

            var handler = update.GetStepHandler<StepTelegram>();
            handler!.GetCache<UserCache>().PersonName = update.Message?.Text;

            // Очікування відповіді від користувача (дата з календаря)
            await Calendar.PickCalendar(botClient, update);

            // Видалення подальшого виклику змінної calendarResponse, оскільки функція PRTelegramBot.Helpers.Message.Send повертає void
            // var calendarResponse = await PRTelegramBot.Helpers.Message.Send(botClient, update, message);

            // Реєстрація наступного кроку
            handler.RegisterNextStep(StepTwo, DateTime.Now.AddMinutes(1));

            Message _ = await PRTelegramBot.Helpers.Message.Send(botClient, update, message);
        }

        public static async Task StepTwo(ITelegramBotClient botClient, Update update)
        {
            string message = $"Entered date {update.Message?.Text}";

            var handler = update.GetStepHandler<StepTelegram>();
            handler!.GetCache<UserCache>().DateT = update.Message?.Text;

            // Чекаємо відповідь користувача (можливо, подібну до підтвердження вибору дати)
            var confirmationResponse = await SomeFunctionToConfirm(botClient, update);

            // Якщо відповідь користувача підтверджує вибір дати, реєструємо наступний крок
            if (confirmationResponse == "confirmed")
            {
                handler.RegisterNextStep(StepThree, DateTime.Now.AddMinutes(1));
            }

            Message _ = await PRTelegramBot.Helpers.Message.Send(botClient, update, message);
        }

        public static async Task StepThree(ITelegramBotClient botClient, Update update)
        {
            var handler = update.GetStepHandler<StepTelegram>();
            var cache = update.GetCacheData<UserCache>();
            var cacheUser = handler!.GetCache<UserCache>();
            cache.DateT = cacheUser.DateT;
            cache.PersonName = cacheUser.PersonName;
            string message = $"Entered date {cacheUser.DateT} and name {cacheUser.PersonName}";

            update.ClearStepUserHandler();

            Message _ = await PRTelegramBot.Helpers.Message.Send(botClient, update, message);
        }

        // Припустима функція для підтвердження вибору користувача
        private static async Task<string> SomeFunctionToConfirm(ITelegramBotClient botClient, Update update)
        {
            // Логіка для підтвердження вибору користувача
            // Можливо, ви хочете викликати інший підкрок або відправити запитання для підтвердження

            // Припустимо, що користувач підтвердив вибір дати
            return "confirmed";
        }
    }
}
