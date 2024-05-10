using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BirthdayReminder.MySqlDataBase.DataBaseConnector
{
    public class PersonInDataBase
    {
        public int Id { get; set; }
        public long TelegramId { get; set; }
        public required string Name { get; set; }
        public DateTime BirthdayDate { get; set; }
    }
}
