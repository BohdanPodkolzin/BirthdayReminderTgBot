using System.Globalization;
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
    public static class Calendar
    {
        private static readonly DateTimeFormatInfo DateTimeFormat =
            CultureInfo.GetCultureInfo("en-GB", false).DateTimeFormat;

        public static async Task PickCalendar(ITelegramBotClient botClient, Update update)
        {
            var calendarMarkup = Markup.Calendar(DateTime.Now, DateTimeFormat);
            var option = new OptionMessage
            {
                MenuInlineKeyboardMarkup = calendarMarkup
            };

            const string message = "<b>Pick a date</b>";
            await PRTelegramBot.Helpers.Message.Send(botClient, update.GetChatId(), message, option);
        }

        [InlineCallbackHandler<THeader>(THeader.YearMonthPicker)]
        public static async Task PickYearMonth(ITelegramBotClient botClient, Update update)
        {
            try
            {
                var command = InlineCallback<CalendarTCommand>.GetCommandByCallbackOrNull(update.CallbackQuery?.Data);
                if (command != null)
                {
                    var monthYearMarkup =
                        Markup.PickMonthYear(command.Data.Date, DateTimeFormat, command.Data.LastCommand);
                    var option = new OptionMessage
                    {
                        MenuInlineKeyboardMarkup = monthYearMarkup
                    };
                    _ = await PRTelegramBot.Helpers.Message.EditInline(botClient, update.CallbackQuery.Message.Chat.Id,
                        update.CallbackQuery.Message.MessageId, option);
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
                    var monthPickerMarkup =
                        Markup.PickMonth(command.Data.Date, DateTimeFormat, command.Data.LastCommand);
                    var option = new OptionMessage
                    {
                        MenuInlineKeyboardMarkup = monthPickerMarkup
                    };
                    _ = await PRTelegramBot.Helpers.Message.EditInline(botClient, update.CallbackQuery.Message.Chat.Id,
                        update.CallbackQuery.Message.MessageId, option);
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
                    var yearPickerMarkup = Markup.PickYear(command.Data.Date, DateTimeFormat, command.Data.LastCommand);
                    var option = new OptionMessage
                    {
                        MenuInlineKeyboardMarkup = yearPickerMarkup
                    };
                    _ = await PRTelegramBot.Helpers.Message.EditInline(botClient, update.CallbackQuery.Message.Chat.Id,
                        update.CallbackQuery.Message.MessageId, option);
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
            if (update.CallbackQuery?.Message is null ||
                update.CallbackQuery.Data is null)
            {
                return;
            }

            try
            {
                var command = InlineCallback<CalendarTCommand>.GetCommandByCallbackOrNull(update.CallbackQuery.Data);
                if (command != null)
                {
                    var calendarMarkup = Markup.Calendar(command.Data.Date, DateTimeFormat);
                    var option = new OptionMessage
                    {
                        MenuInlineKeyboardMarkup = calendarMarkup
                    };
                    await PRTelegramBot.Helpers.Message.EditInline(botClient, update.CallbackQuery.Message.Chat.Id,
                        update.CallbackQuery.Message.MessageId, option);
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
                    var data = command.Data.Date;

                    var message = $"Picked date: <b>{data:dd.MM.yyyy}</b>";
                    await PRTelegramBot.Helpers.Message.Edit(botClient, update, message);

                    //caching date
                    var cache = update.GetCacheData<UserCache>();
                    cache.DateT = data;
                    CacheCommand.UpdateCache(update, cache.PersonName ?? "unknown", cache.DateT);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
