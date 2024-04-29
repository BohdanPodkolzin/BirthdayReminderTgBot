using System.Globalization;
using BirthdayReminder.Enums;
using PRTelegramBot.Interface;
using PRTelegramBot.Models;
using PRTelegramBot.Models.CallbackCommands;
using PRTelegramBot.Models.InlineButtons;
using PRTelegramBot.Utils;
using PRTelegramBot.Utils.Controls.CalendarControl.Common;
using Telegram.Bot.Types.ReplyMarkups;

namespace BirthdayReminder.Helpers;

public static class InlineKeyboardsHelper
{
    public static InlineKeyboardMarkup ConfirmationKeyboard()
    {
        var yesButton = new InlineCallback("Yes", ConfirmationTHeader.Yes);
        var noButton = new InlineCallback("No", ConfirmationTHeader.No);

        var yesOrNo = new List<IInlineContent>
        {
            yesButton,
            noButton
        };
        return MenuGenerator.InlineKeyboard(2, yesOrNo);
    }

    public static InlineKeyboardMarkup MenuKeyboard()
    {
        var addButton = new InlineCallback("Add Countdown", EditCountdownTHeader.Add);
        var delButton = new InlineCallback("Delete Countdown", EditCountdownTHeader.Del);
        var allDelButton = new InlineCallback("Delete All Schedule", EditCountdownTHeader.AllDel);

        List<IInlineContent> menu =
        [
            addButton,
            delButton,
            allDelButton
        ];

        return MenuGenerator.InlineKeyboard(2, menu);
    }

    public static OptionMessage AsOption(this InlineKeyboardMarkup keyboardMarkup)
        => new() { MenuInlineKeyboardMarkup = keyboardMarkup };

    public static class Calendar
    {
        private static readonly DateTimeFormatInfo DateTimeFormat =
            CultureInfo.GetCultureInfo("en-GB", false).DateTimeFormat;

        public static InlineKeyboardMarkup PickMonth(InlineCallback<CalendarTCommand> command)
            => Markup.PickMonth(command.Data.Date, DateTimeFormat, command.Data.LastCommand);

        public static InlineKeyboardMarkup PickYear(InlineCallback<CalendarTCommand> command)
            => Markup.PickYear(command.Data.Date, DateTimeFormat, command.Data.LastCommand);

        public static InlineKeyboardMarkup PickMonthYear(InlineCallback<CalendarTCommand> command)
            => Markup.PickMonthYear(command.Data.Date, DateTimeFormat, command.Data.LastCommand);

        public static InlineKeyboardMarkup PickCalendar(InlineCallback<CalendarTCommand> command)
            => Markup.Calendar(command.Data.Date, DateTimeFormat);

        public static InlineKeyboardMarkup PickCalendar(DateTime from)
            => Markup.Calendar(from, DateTimeFormat);
    }
}
