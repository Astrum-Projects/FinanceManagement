using Telegram.Bot.Types;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using Application.Helper;

namespace Application.Services.BotServices
{
    public partial class UpdateHandlerService
    {
        private async Task HandleMessageAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var user = await update.UserAsync();

            if (update.Message == null) throw new ArgumentNullException(nameof(update)+ "type unknown");
            var message = update.Message;

            var messageHandler = message.Text switch
            {
                "/start" => HandleStartCommandAsync(botClient, update, cancellationToken),
            };

            try
            {
                await messageHandler;
            }
            catch (Exception ex) 
            {
                throw;
            }
        }

        private async Task HandleStartCommandAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var buttons = new List<InlineKeyboardButton>()
            {
                InlineKeyboardButton.WithCallbackData("🇺🇿 Uzbek",$"lan uz"),
                InlineKeyboardButton.WithCallbackData("🇷🇺 Russian",$"lan ru"),
                InlineKeyboardButton.WithCallbackData("🇺🇸 English",$"lan en"),
            };

            await botClient.SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text: "Tilni tanlang!",
                replyMarkup: new InlineKeyboardMarkup(buttons),
                cancellationToken: cancellationToken);
        }
    }
}
