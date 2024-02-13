using PRTelegramBot.Attributes;
using PRTelegramBot.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using tg.UsersCache;

namespace tg
{
    public class InfinityLoop
    {
        private readonly TimeSpan _checkInterval = TimeSpan.FromHours(6); // Every 12 hours check again

        [ReplyMenuHandler("/go")]
        public async Task StartReminderLoop(ITelegramBotClient botClient, Update update)
        {
            for (;true;)
            {
                await ReminderBack.RemindPersonForBirthday(botClient, update);
                await Task.Delay(_checkInterval);
            }
        }


    }
}
