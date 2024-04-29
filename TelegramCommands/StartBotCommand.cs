using BirthdayReminder.PersonReminder;
using PRTelegramBot.Attributes;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BirthdayReminder.TelegramCommands
{
    public static class StartBotCommand
    {
        [ReplyMenuHandler("/start")]
        public static async Task StartBotMethod(ITelegramBotClient botClient, Update update)
        {
            if (update.Message?.From != null)
            {
                var user = update.Message.From;
                var userNickName = user?.Username ?? "";

                var startMessage = $"🖐️ Hey, @{userNickName}!\nTo make your first schedule of bithdays enter /menu";
                var _ = await PRTelegramBot.Helpers.Message.Send(botClient, update, startMessage);
                await InfinityLoop.StartReminderLoop(botClient, update);
            }

        }
    }
}
