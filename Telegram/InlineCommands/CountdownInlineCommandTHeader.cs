using System.ComponentModel;
using PRTelegramBot.Attributes;

namespace BirthdayReminder.Telegram.InlineCommands
{
    [InlineCommand]
    public enum CountdownInlineCommandTHeader
    {
        [Description("Add Button")]
        Add = 500,
        [Description("Del Button")]
        Del,
        [Description("All Del Button")]
        AllDel,
    }
}
