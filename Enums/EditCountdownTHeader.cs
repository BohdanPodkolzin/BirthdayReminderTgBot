﻿using System.ComponentModel;
using PRTelegramBot.Attributes;

namespace BirthdayReminder.Enums
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