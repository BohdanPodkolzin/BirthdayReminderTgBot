using PRTelegramBot.Attributes;
using PRTelegramBot.Interface;
using PRTelegramBot.Models.InlineButtons;
using PRTelegramBot.Models;
using PRTelegramBot.Utils;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot;
using tg.Models;
using Telegram.Bot.Types;


namespace tg.TelegramCommands
{
    public class AllSceduleConfrim
    {
        [InlineCallbackHandler<EditCountdownTHeader>(EditCountdownTHeader.AllDel)]
        public static async Task Confirm(ITelegramBotClient botClient, Update update)
        {
            var message = "Confirm deleting all list";

            var yesButton = new InlineCallback("Yes", ConfirmationTHeader.Yes);
            var noButton = new InlineCallback("No", ConfirmationTHeader.No);

            List<IInlineContent> yesOrNo = new List<IInlineContent>();
            yesOrNo.Add(yesButton);
            yesOrNo.Add(noButton);

            InlineKeyboardMarkup inlineKeyboard = MenuGenerator.InlineKeyboard(2, yesOrNo);

            OptionMessage option = new OptionMessage();
            option.MenuInlineKeyboardMarkup = inlineKeyboard;

            await PRTelegramBot.Helpers.Message.Send(botClient, update, message, option);
        }
    }
}
