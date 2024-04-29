using System.Globalization;
using BirthdayReminder.Models;
using BirthdayReminder.UsersCache;
using PRTelegramBot.Attributes;
using PRTelegramBot.Extensions;
using PRTelegramBot.Models;
using PRTelegramBot.Models.CallbackCommands;
using PRTelegramBot.Models.InlineButtons;
using PRTelegramBot.Utils.Controls.CalendarControl.Common;
using Telegram.Bot;
using Telegram.Bot.Types;
using THeader = PRTelegramBot.Models.Enums.THeader;

namespace BirthdayReminder.Calendar
{
    public class Calendar
    {

        public static DateTimeFormatInfo dtfi = CultureInfo.GetCultureInfo("en-GB", false).DateTimeFormat;


        public static async Task PickCalendar(ITelegramBotClient botClient, Update update)
        {
            var calendarMarkup = Markup.Calendar(DateTime.Now, dtfi); // dtfi is DateTimeFormatInfo
            var option = new OptionMessage();
            option.MenuInlineKeyboardMarkup = calendarMarkup;

            Message _ = await PRTelegramBot.Helpers.Message.Send(botClient, update.GetChatId(), "<b>Pick a date</b>", option);
        }


        [InlineCallbackHandler<THeader>(THeader.YearMonthPicker)]
        public static async Task PickYeadMonth(ITelegramBotClient botClient, Update update)
        {
            try
            {
                var command = InlineCallback<CalendarTCommand>.GetCommandByCallbackOrNull(update.CallbackQuery?.Data);
                if (command != null)
                {
                    var monthYearMarkup = Markup.PickMonthYear(command.Data.Date, dtfi, command.Data.LastCommand);
                    var option = new OptionMessage();
                    option.MenuInlineKeyboardMarkup = monthYearMarkup;
                    Message _ = await PRTelegramBot.Helpers.Message.EditInline(botClient, update.CallbackQuery.Message.Chat.Id, update.CallbackQuery.Message.MessageId, option);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }

        [InlineCallbackHandler<THeader>(THeader.PickMonth)]
        public static async Task PickMonth(ITelegramBotClient botClient, Update update)
        {
            try
            {
                var command = InlineCallback<CalendarTCommand>.GetCommandByCallbackOrNull(update.CallbackQuery?.Data);
                if (command != null)
                {
                    var monthPickerMarkup = Markup.PickMonth(command.Data.Date, dtfi, command.Data.LastCommand);
                    var Option = new OptionMessage();
                    Option.MenuInlineKeyboardMarkup = monthPickerMarkup;
                    Message _ = await PRTelegramBot.Helpers.Message.EditInline(botClient, update.CallbackQuery.Message.Chat.Id, update.CallbackQuery.Message.MessageId, Option);

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        [InlineCallbackHandler<THeader>(THeader.PickYear)]
        public static async Task PickYear(ITelegramBotClient botClient, Update update)
        {
            try
            {
                var command = InlineCallback<CalendarTCommand>.GetCommandByCallbackOrNull(update.CallbackQuery?.Data);
                if (command != null)
                {
                    var yearPickerMarkup = Markup.PickYear(command.Data.Date, dtfi, command.Data.LastCommand);
                    var Option = new OptionMessage();
                    Option.MenuInlineKeyboardMarkup = yearPickerMarkup;
                    Message _ = await PRTelegramBot.Helpers.Message.EditInline(botClient, update.CallbackQuery.Message.Chat.Id, update.CallbackQuery.Message.MessageId, Option);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }


        [InlineCallbackHandler<THeader>(THeader.ChangeTo)]
        public static async Task ChangeToHandler(ITelegramBotClient botClient, Update update)
        {
            try
            {
                var command = InlineCallback<CalendarTCommand>.GetCommandByCallbackOrNull(update.CallbackQuery?.Data);
                if (command != null)
                {
                    var calendarMarkup = Markup.Calendar(command.Data.Date, dtfi);
                    var option = new OptionMessage();
                    option.MenuInlineKeyboardMarkup = calendarMarkup;
                    Message _ = await PRTelegramBot.Helpers.Message.EditInline(botClient, update.CallbackQuery.Message.Chat.Id, update.CallbackQuery.Message.MessageId, option);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }


        [InlineCallbackHandler<THeader>(THeader.PickDate)]
        public static async Task PickDate(ITelegramBotClient botClient, Update update)
        {
            try
            {
                var command = InlineCallback<CalendarTCommand>.GetCommandByCallbackOrNull(update.CallbackQuery?.Data);
                if (command != null)
                {
                    var type = command.Data.GetLastCommandEnum<EditCountdownTHeader>();
                    var data = command.Data.Date;

                    string message = $"Picked date: <b>{data.ToString("dd.MM.yyyy")}</b>";
                    Message showDate = await PRTelegramBot.Helpers.Message.Edit(botClient, update, message);


                    //caching date
                    var cache = update.GetCacheData<UserCache>();
                    cache.DateT = data;
                    await CacheCommand.UpdateCache(update, cache.PersonName ?? "unknown", cache.DateT);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }




    }
}