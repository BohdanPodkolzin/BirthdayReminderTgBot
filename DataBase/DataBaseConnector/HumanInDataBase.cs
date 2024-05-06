using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BirthdayReminder.DataBase.DataBaseConnector
{
    public class HumanInDataBase
    {
        public int Id { get; set; }
        public long TelegramId { get; set; }
        public required string HumanInSchedule { get; set; }
        public DateTime BirthdayDate { get; set; }
    }
}
