using Telegram.Bot.Types;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using Application.Helper;
using User = Domain.Entities.User;
using Domain.Entities;
using Domain.Enums;
using System.Text.Json;
using System.Text;

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

            if(user is not null)
            {
                if (_state.GetState(user.TelegramId).state == EUserState.Amount)
                {
                    await HandleAmountAsync(botClient, update, user, cancellationToken);
                    return;
                }
            }

            var messageHandler = message.Text switch
            {
                "/start" => HandleStartCommandAsync(botClient, update, user, cancellationToken),
                "/lan" => HandleChangeLanguageCommandAsync(botClient, update, user, cancellationToken),
                _ => HandleNimadur(botClient, update, user, cancellationToken)
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

        private async Task HandleNimadur(ITelegramBotClient botClient, Update update, User? user, CancellationToken cancellationToken)
        {
            var text = update.Message.Text;

            if(!(text.StartsWith("+") || text.StartsWith("-")))
            {
                return;
            }

            List<Transfer> transfers; 

            if (text.StartsWith("+"))
                transfers = await _transferRepository.GetAllTransfersAsync(user.Id, true);
            else
                transfers = await _transferRepository.GetAllTransfersAsync(user.Id, false);

            StringBuilder transferList = new StringBuilder("Transferlar \n");

            foreach (var transfer in transfers)
            {
                transferList.AppendLine($"```{transfer.Amount}``` - {transfer.Category.GetLocalizedName(user.LanguageCode)} \n");
            }

            await botClient.SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text: transferList.ToString(),
                parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown,
                cancellationToken: cancellationToken);
        }

        private async Task HandleChangeLanguageCommandAsync(ITelegramBotClient botClient, Update update, User? user, CancellationToken cancellationToken)
        {
            var buttons = new List<InlineKeyboardButton>()
            {
                InlineKeyboardButton.WithCallbackData("🇺🇿 Uzbek",$"select-lan uz"),
                InlineKeyboardButton.WithCallbackData("🇷🇺 Russian",$"select-lan ru"),
                InlineKeyboardButton.WithCallbackData("🇺🇸 English",$"select-lan en"),
            };

            await botClient.SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text: Localization.GetLocalizedCommand("select", user is not null ? user.LanguageCode : "en"),
                replyMarkup: new InlineKeyboardMarkup(buttons),
                cancellationToken: cancellationToken);
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

            var transfers = await _transferRepository.GetAllTransfersAsync(user.Id);

            await botClient.SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text: JsonSerializer.Serialize(transfers),
                cancellationToken: cancellationToken);
        }

        private async Task HandleStartCommandAsync(ITelegramBotClient botClient, Update update, User user, CancellationToken cancellationToken)
        {
            List<InlineKeyboardButton> buttons;

            if (user is null)
            {
                buttons = new List<InlineKeyboardButton>()
                {
                    InlineKeyboardButton.WithCallbackData("🇺🇿 Uzbek",$"select-lan uz"),
                    InlineKeyboardButton.WithCallbackData("🇷🇺 Russian",$"select-lan ru"),
                    InlineKeyboardButton.WithCallbackData("🇺🇸 English",$"select-lan en"),
                };
            }
            else
            {
                buttons = new List<InlineKeyboardButton>() {
                    InlineKeyboardButton.WithCallbackData(
                        text: Localization.GetLocalizedCommand("income",user.LanguageCode),
                        callbackData: "select-transfer-type +"),
                    InlineKeyboardButton.WithCallbackData(
                        text: Localization.GetLocalizedCommand("expense",user.LanguageCode),
                        callbackData: "select-transfer-type -")
                };
            }

            await botClient.SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text: Localization.GetLocalizedCommand("select", user is not null ? user.LanguageCode: "en"),
                replyMarkup: new InlineKeyboardMarkup(buttons),
                cancellationToken: cancellationToken);
        }
    }
}
