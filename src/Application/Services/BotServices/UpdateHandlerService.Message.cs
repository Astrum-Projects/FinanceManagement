using Telegram.Bot.Types;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using Application.Helper;
using User = Domain.Entities.User;
using System.Text.RegularExpressions;
using Domain.Entities;
using Domain.Enums;
using System.Text.Json;

namespace Application.Services.BotServices
{
    public partial class UpdateHandlerService
    {
        private async Task HandleMessageAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var user = await update.UserAsync();

            if (update.Message == null) throw new ArgumentNullException(nameof(update)+ "type unknown");
            if (update.Message.Text == null) throw new ArgumentNullException(nameof(update)+ "type unknown");

            var message = update.Message;

            if (_state.GetState(user.TelegramId).state == EUserState.Amount)
            {
                await HandleAmountAsync(botClient, update, user, cancellationToken);
                return;
            }

            var messageHandler = message.Text switch
            {
                "/start" => HandleStartCommandAsync(botClient, update, user, cancellationToken),
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

        private async Task HandleAmountAsync(ITelegramBotClient botClient, Update update, User user, CancellationToken cancellationToken)
        {
            var amountString = update.Message.Text;

            if (decimal.TryParse(amountString, out decimal amount))
            {
                if (amount <= 0)
                {
                    await botClient.SendTextMessageAsync(
                        chatId: update.Message.Chat.Id,
                        text: Localization.GetLocalizedCommand("invalid-format", user.LanguageCode),
                        cancellationToken: cancellationToken);
                    return;
                }
                else
                {
                    var transfer = new Transfer()
                    {
                        Amount = amount,
                        UserId = user.Id,
                        CategoryId = _state.GetState(user.TelegramId).categoryId.Value,
                        Comment = "Comment"
                    };

                    await _transferRepository.CreateTransferAsync(transfer);

                    _state.SetState(user.TelegramId,null,null);
                }
            }
            else
            {
                await botClient.SendTextMessageAsync(
                    chatId: update.Message.Chat.Id,
                    text: Localization.GetLocalizedCommand("invalid-format", user.LanguageCode),
                    cancellationToken: cancellationToken);
            }

            var transfers = await _transferRepository.GetAllTransfersAsync();

            await botClient.SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text: JsonSerializer.Serialize(transfers),
                cancellationToken: cancellationToken);
        }

        private async Task HandleStartCommandAsync(ITelegramBotClient botClient, Update update, User user, CancellationToken cancellationToken)
        {
            var buttons = new List<InlineKeyboardButton>()
            {
                InlineKeyboardButton.WithCallbackData("🇺🇿 Uzbek",$"select-lan uz"),
                InlineKeyboardButton.WithCallbackData("🇷🇺 Russian",$"select-lan ru"),
                InlineKeyboardButton.WithCallbackData("🇺🇸 English",$"select-lan en"),
            };

            await botClient.SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text: "Choose language!",
                replyMarkup: new InlineKeyboardMarkup(buttons),
                cancellationToken: cancellationToken);
        }
    }
}
