using PRTelegramBot.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tg.Models
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
