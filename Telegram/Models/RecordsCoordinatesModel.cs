using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PRTelegramBot.Interface;

namespace BirthdayReminder.Telegram.Models
{
    public class RecordsCoordinatesModel : ITelegramCache
    {
        public int Id { get; set; }

        public long TelegramId { get; set; }

        public string? Latitude { get; set; }
        public string? Longitude { get; set; }

        public bool ClearData()
        {
            Id = 0;
            TelegramId = 0;
            Latitude = string.Empty;
            Longitude = string.Empty;
            return true;
        }
    }
}
