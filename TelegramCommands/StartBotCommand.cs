using PRTelegramBot.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot;
using tg.PersonReminder;

namespace tg.TelegramCommands
{
    public static class StartBotCommand
    {
        [ReplyMenuHandler("/start")]
        public static async Task StartBotMethod(ITelegramBotClient botClient, Update update)
        {
            if (update.Message?.From != null)
            {
                User user = update.Message.From;
                string userNickName = user?.Username ?? "";

                if (userNickName != null)
                {
                    string startMessage = $"🖐️ Hey, @{userNickName}!\nTo make your first schedule of bithdays enter /menu";
                    Message _ = await PRTelegramBot.Helpers.Message.Send(botClient, update, startMessage);
                    await InfinityLoop.StartReminderLoop(botClient, update);
                }
            }

        }
    }
}
