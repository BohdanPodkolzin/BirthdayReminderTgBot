using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRTelegramBot.Interface;

namespace BirthdayReminder.Telegram.Models
{
    public class PlaceCoordinatesStepCache : ITelegramCache
    {
        public readonly Dictionary<long, (string, string)> PlaceCoordinatesDict = new();

        public long UserId { get; set; }
        public string? Latitude { get; set; }
        public string? Longitude { get; set; }

        public bool ClearData()
        {
            PlaceCoordinatesDict.Clear();
            UserId = 0;
            Latitude = null;
            Longitude = null;
            return true;
        }
    }
}
