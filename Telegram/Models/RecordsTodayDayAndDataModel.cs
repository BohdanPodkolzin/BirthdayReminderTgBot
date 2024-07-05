using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BirthdayReminder.Telegram.Models
{
    public class RecordsTodayDayAndDataModel
    {
        public long TelegramId { get; set; }
        public string? Name { get; set; }
        public DateTime? BirthdayDate { get; set; }
        public DateTime TodayDate { get; set; }
        public string? Latitude { get; set; }
        public string? Longitude { get; set; }
    }
}
