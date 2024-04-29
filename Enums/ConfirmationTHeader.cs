using System.ComponentModel;
using PRTelegramBot.Attributes;

namespace BirthdayReminder.Enums
{
    [InlineCommand]
    public enum ConfirmationTHeader
    {
        [Description("Yes")]
        Yes = 601,
        [Description("No")]
        No,
    }
}
