using PRTelegramBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace tg
{
    public class UserCache : TelegramCache, ITelegramCache
    {
        public long Id { get; set; }
        public string Data { get; set; }
        public bool ClearData()
        {
            Id = 0;
            Data = "";
            return true;
        }
    }
}
