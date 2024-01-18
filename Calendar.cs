using Telegram.Bot;
using Telegram.Bot.Types;
using PRTelegramBot.Attributes;
using PRTelegramBot.Utils.Controls.CalendarControl.Common;
using PRTelegramBot.Extensions;
using System.Globalization;
using THeader = PRTelegramBot.Models.Enums.THeader;
using PRTelegramBot.Models.InlineButtons;
using PRTelegramBot.Models;
using PRTelegramBot.Models.CallbackCommands;
using System.Security.Cryptography.X509Certificates;

namespace tg
{
    public class Calendar
    {

        public static DateTimeFormatInfo dtfi = CultureInfo.GetCultureInfo("en-GB", false).DateTimeFormat;


        [InlineCallbackHandler<EditCountdownTHeader>(EditCountdownTHeader.Add)]
        public static async Task PickCalendar(ITelegramBotClient botClient, Update update)
        {
            var calendarMarkup = Markup.Calendar(DateTime.Now, dtfi); // dtfi is DateTimeFormatInfo
            var option = new OptionMessage();
            option.MenuInlineKeyboardMarkup = calendarMarkup;

            Message _ = await PRTelegramBot.Helpers.Message.Send(botClient, update.GetChatId(), "Pick a date", option);
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


                    Message showDate = await PRTelegramBot.Helpers.Message.Send(botClient, update, data.ToString("dd.MM.yyyy"));

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }




    }
}