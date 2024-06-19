using PRTelegramBot.Attributes;
using Telegram.Bot;
using Telegram.Bot.Types;
using static BirthdayReminder.MySqlDataBase.DataBaseConnector.Queries;
using static BirthdayReminder.Telegram.CommandHandlers.StartBotCommand.NewUserStartBotHandler;
using static BirthdayReminder.Telegram.CommandHandlers.StartBotCommand.ExistingUserStartBotHandler;

namespace BirthdayReminder.Telegram.CommandHandlers.StartBotCommand
{
    public class StartBotCommandHandler
    {

        [ReplyMenuHandler("/start")]
        public static async Task StartBotNewMethod(ITelegramBotClient botClient, Update update)
        {
            if (update.Message?.From == null) return;

            if (!await IsUserExist(update.Message.From.Id))
            {
                await NewUserStartBot(botClient, update);
            }
            else
            {
                await ExistingUserStartBotMenu(botClient, update);
            }
        }
    }
}
