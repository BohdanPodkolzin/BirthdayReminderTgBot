using System.Globalization;
using BirthdayReminder.Telegram.InlineCommands;
using PRTelegramBot.Interface;
using PRTelegramBot.Models;
using PRTelegramBot.Models.CallbackCommands;
using PRTelegramBot.Models.InlineButtons;
using PRTelegramBot.Utils;
using PRTelegramBot.Utils.Controls.CalendarControl.Common;
using Telegram.Bot.Types.ReplyMarkups;

namespace BirthdayReminder.Telegram.Helpers;

public static class InlineKeyboardsHelper
{
    public static InlineKeyboardMarkup ConfirmationKeyboard()
    {
        var yesButton = new InlineCallback("Yes", ConfirmationInlineCommandTHeader.Yes);
        var noButton = new InlineCallback("No", ConfirmationInlineCommandTHeader.No);

        var yesOrNo = new List<IInlineContent>
        {
            yesButton,
            noButton
        };
        return MenuGenerator.InlineKeyboard(2, yesOrNo);
    }

    public static InlineKeyboardMarkup CountdownMenuKeyboard()
    {
        var addButton = new InlineCallback("Add Countdown", CountdownInlineCommandTHeader.Add);
        var delButton = new InlineCallback("Delete Countdown", CountdownInlineCommandTHeader.Del);
        var allDelButton = new InlineCallback("Delete All Schedule", CountdownInlineCommandTHeader.AllDel);

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
