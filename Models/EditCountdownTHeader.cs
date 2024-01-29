using PRTelegramBot.Attributes;
using System.ComponentModel;


namespace tg.Models
{
    [InlineCommand]
    public enum EditCountdownTHeader
    {
        [Description("Add Button")]
        Add = 500,
        [Description("Del Button")]
        Del,
        [Description("All Del Button")]
        AllDel,
    }
}