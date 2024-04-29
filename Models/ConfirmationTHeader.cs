using System.ComponentModel;
using PRTelegramBot.Attributes;

namespace BirthdayReminder.Models
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