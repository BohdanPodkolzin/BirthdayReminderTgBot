using System.ComponentModel;
using PRTelegramBot.Attributes;

namespace BirthdayReminder.Telegram.InlineCommands
{
    [InlineCommand]
    public enum ConfirmationInlineCommandTHeader
    {
        [Description("Yes")]
        Yes = 601,
        [Description("No")]
        No,
    }
}
