using BirthdayReminder.PersonReminder;
using PRTelegramBot.Attributes;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BirthdayReminder.Telegram.CommandHandlers
{
    public static class StartBotCommandHandler
    {
        [ReplyMenuHandler("/start")]
        public static async Task StartBotMethod(ITelegramBotClient botClient, Update update)
        {
            if (update.Message?.From == null)
            {
                return;
            }

            var user = update.Message.From;
            var userNickName = user?.Username ?? "";

            var startMessage = $"🖐️ Hey, @{userNickName}!\n" +
                               $"To make your first schedule of birthdays enter /menu";

            
            await DataBase.DataBaseConnector.MySqlConnector.UpdateDataInDataBase(update.Message.From.Id, "Halya", DateTime.MinValue);
            await DataBase.DataBaseConnector.MySqlConnector.ReadFullDataFromDataBase();
            await PRTelegramBot.Helpers.Message.Send(botClient, update, startMessage);
            await InfinityLoop.StartReminderLoop(botClient, update);
        }
    }
}
