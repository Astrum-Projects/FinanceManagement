using Telegram.Bot.Types;
using Telegram.Bot;
using Application.Helper;
using User = Domain.Entities.User;
using Telegram.Bot.Types.ReplyMarkups;

namespace Application.Services.BotServices
{
    public partial class UpdateHandlerService
    {
        public async Task HandleCallbackQueryAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var user = await update.GetUserAsync();
            var callbackData = update.CallbackQuery.Data;
            var keys = callbackData.Split(' ');

            var callbackHandler = keys[0] switch
            {
                "lan" => HandleLanguageSelectAsync(botClient, update, user, keys, cancellationToken),
                "transfertype" => HandleTransferTypeSelectAsync(botClient, update, user, keys, cancellationToken),
                "category" => HandleCategoryTypeSelectAsync(botClient, update, user, keys, cancellationToken),
            };

            try
            {
                await callbackHandler;
            }
            catch
            {
                throw;
            }

        }

        private async Task HandleCategoryTypeSelectAsync(ITelegramBotClient botClient, Update update, User user, string[] keys, CancellationToken cancellationToken)
        {
            await botClient.SendTextMessageAsync(
                chatId: update.CallbackQuery.Message.Chat.Id,
                text: Localization.GetLocalizedCommand("enter-summa", user.LanguageCode),
                cancellationToken: cancellationToken);
        }

        private async Task HandleTransferTypeSelectAsync(ITelegramBotClient botClient, Update update, User user, string[] keys, CancellationToken cancellationToken)
        {
            // faqat shu user uchun categorylarni olib kel
            var categories = await _categoryRepository.GetAllAsync();

            var buttons = categories.Select(x => new List<InlineKeyboardButton> { InlineKeyboardButton.WithCallbackData(x.GetLocalizedName(user.LanguageCode), $"category {x.Id}") }).ToList();

            await botClient.SendTextMessageAsync(
                chatId: update.CallbackQuery.Message.Chat.Id,
                text: Localization.GetLocalizedCommand("select-category", user.LanguageCode),
                replyMarkup: new InlineKeyboardMarkup(buttons),
                cancellationToken: cancellationToken);
        }

        private async Task HandleLanguageSelectAsync(ITelegramBotClient botClient, Update update, User user, string[] keys, CancellationToken cancellationToken)
        {
            if (user == null) 
            {
                user = new User()
                {
                    FirstName = update.CallbackQuery.From.FirstName,
                    LastName = update.CallbackQuery.From.LastName,
                    Username = update.CallbackQuery.From.Username,
                    TelegramId = update.CallbackQuery.From.Id,
                    LanguageCode = keys[1],
                };

                await _userRepository.CreateOrModifyUser(user);
            }
            else
            {
                user.LanguageCode = keys[1];
                await _userRepository.CreateOrModifyUser(user);
            }

            await botClient.SendTextMessageAsync(
                chatId: update.CallbackQuery.Message.Chat.Id,
                text: Localization.GetLocalizedCommand("hello",user.LanguageCode),
                cancellationToken: cancellationToken);

            var buttons = new List<InlineKeyboardButton>()
            {
                InlineKeyboardButton.WithCallbackData(Localization.GetLocalizedCommand("income",user.LanguageCode),"transfertype +"),
                InlineKeyboardButton.WithCallbackData(Localization.GetLocalizedCommand("expense",user.LanguageCode),"transfertype -"),
            };

            await botClient.SendTextMessageAsync(
                chatId: update.CallbackQuery.Message.Chat.Id,
                text: Localization.GetLocalizedCommand("select-transfer-type", user.LanguageCode),
                replyMarkup: new InlineKeyboardMarkup(buttons),
                cancellationToken: cancellationToken);
        }
    }
}
