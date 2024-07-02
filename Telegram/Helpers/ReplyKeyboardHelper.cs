using PRTelegramBot.Models;
using PRTelegramBot.Utils;
using Telegram.Bot.Types.ReplyMarkups;

namespace BirthdayReminder.Telegram.Helpers;

public static class ReplyKeyboardHelper
{
    public static ReplyKeyboardMarkup GetBotMenu()
    {
        List<KeyboardButton> menuList =
        [
            new KeyboardButton("About"),
            new KeyboardButton("Edit Timezone"),
            new KeyboardButton("Edit Countdown"),
            new KeyboardButton("Show countdown")
        ];

        return MenuGenerator.ReplyKeyboard(2, menuList);
    }

    public static ReplyKeyboardMarkup GetConfirmationTimezoneMenu()
    {
        List<KeyboardButton> menuList =
        [
            new KeyboardButton("Confirm"),
            new KeyboardButton("Edit")  
        ];

        return MenuGenerator.ReplyKeyboard(2, menuList);
    }

    public static OptionMessage AsOption(this ReplyKeyboardMarkup keyboardMarkup)
        => new() { MenuReplyKeyboardMarkup = keyboardMarkup };
}
