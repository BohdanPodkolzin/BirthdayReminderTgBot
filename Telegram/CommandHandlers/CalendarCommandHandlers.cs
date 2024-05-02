using BirthdayReminder.Telegram.Helpers;
using BirthdayReminder.Telegram.Models;
using PRTelegramBot.Attributes;
using PRTelegramBot.Extensions;
using PRTelegramBot.Models;
using PRTelegramBot.Models.CallbackCommands;
using PRTelegramBot.Models.InlineButtons;
using Telegram.Bot;
using Telegram.Bot.Types;
using THeader = PRTelegramBot.Models.Enums.THeader;

namespace BirthdayReminder.Telegram.CommandHandlers
{
    public static class CalendarCommandHandlers
    {
        public static async Task PickCalendar(ITelegramBotClient botClient, Update update)
        {
            var option = InlineKeyboardsHelper.Calendar.PickCalendar(DateTime.Now).AsOption();

            const string message = "<b>Pick a date</b>";
            await PRTelegramBot.Helpers.Message.Send(botClient, update.GetChatId(), message, option);
        }

        #region User Choice Handlers

        [InlineCallbackHandler<THeader>(THeader.YearMonthPicker)]
        public static async Task PickYearMonth(ITelegramBotClient botClient, Update update)
            => await PickBase(botClient, update, THeader.YearMonthPicker, GetKeyboard);

        [InlineCallbackHandler<THeader>(THeader.PickMonth)]
        public static async Task PickMonth(ITelegramBotClient botClient, Update update)
            => await PickBase(botClient, update, THeader.PickMonth, GetKeyboard);

        [InlineCallbackHandler<THeader>(THeader.PickYear)]
        public static async Task PickYear(ITelegramBotClient botClient, Update update)
            => await PickBase(botClient, update, THeader.PickYear, GetKeyboard);

        [InlineCallbackHandler<THeader>(THeader.ChangeTo)]
        public static async Task ChangeToHandler(ITelegramBotClient botClient, Update update)
            => await PickBase(botClient, update, THeader.ChangeTo, GetKeyboard);

        [InlineCallbackHandler<THeader>(THeader.PickDate)]
        public static async Task PickDate(ITelegramBotClient botClient, Update update)
        {
            if (update.CallbackQuery?.Data is null)
            {
                return;
            }

            try
            {
                string message;
                var command = InlineCallback<CalendarTCommand>
                    .GetCommandByCallbackOrNull(update.CallbackQuery.Data);

                var data = command.Data.Date;

                if (data > DateTime.Now)
                {
                    message = "Oops.. You are trying to pick a future date..\nEnter another one";
                }
                else
                {
                    message = $"Picked date: <b>{data:dd.MM.yyyy}</b>";

                    // caching the date
                    var cache = update.GetCacheData<UserCache>();
                    cache.DateT = data;
                    CacheCommand.UpdateCache(update, cache.PersonName ?? "unknown", cache.DateT);
                }

                await PRTelegramBot.Helpers.Message.Edit(botClient, update, message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        #endregion

        private static async Task PickBase(
            ITelegramBotClient botClient, Update update, THeader header,
            Func<THeader, InlineCallback<CalendarTCommand>, OptionMessage> getKeyboard)
        {
            if (update.CallbackQuery?.Data is null)
            {
                return;
            }

            try
            {
                var chatId = update.GetChatId();
                var messageId = update.GetMessageId();

                var command = InlineCallback<CalendarTCommand>.GetCommandByCallbackOrNull(update.CallbackQuery.Data);
                if (command is not { } __)
                {
                    return;
                }

                var option = getKeyboard(header, command);
                await PRTelegramBot.Helpers.Message.EditInline(
                    botClient, chatId, messageId, option);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private static OptionMessage GetKeyboard(THeader header, InlineCallback<CalendarTCommand> command)
        {
            return header switch
            {
                THeader.PickYear => InlineKeyboardsHelper.Calendar.PickYear(command).AsOption(),
                THeader.PickMonth => InlineKeyboardsHelper.Calendar.PickMonth(command).AsOption(),
                THeader.ChangeTo => InlineKeyboardsHelper.Calendar.PickCalendar(command).AsOption(),
                THeader.YearMonthPicker => InlineKeyboardsHelper.Calendar.PickMonthYear(command).AsOption(),
                _ => throw new ArgumentOutOfRangeException(nameof(header), header, null)
            };
        }
    }
}
