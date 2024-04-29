using BirthdayReminder.Enums;
using PRTelegramBot.Interface;
using PRTelegramBot.Models.InlineButtons;
using PRTelegramBot.Utils;
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
}
