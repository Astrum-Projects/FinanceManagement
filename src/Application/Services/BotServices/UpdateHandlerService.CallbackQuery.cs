using Telegram.Bot.Types;
using Telegram.Bot;
using Application.Helper;
using User = Domain.Entities.User;
using Telegram.Bot.Types.ReplyMarkups;
using Domain.Entities;
using Domain.Enums;

namespace Application.Services.BotServices
{
    public partial class UpdateHandlerService
    {
        public async Task HandleCallbackQueryAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var user = await update.UserAsync();

            var callbackData = update.CallbackQuery.Data;
            var keys = callbackData.Split(' ');

            var callbackHandler = keys[0] switch
            {
                "select-lan" => HandleLanguageSelectAsync(botClient, update, user, keys, cancellationToken),
                "select-transfer-type" => HandleTransferTypeSelectAsync(botClient, update, user, keys, cancellationToken),
                "select-category" => HandleCategorySelectAsync(botClient, update, user, keys, cancellationToken),
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

        private async Task HandleCategorySelectAsync(ITelegramBotClient botClient, Update update, User user, string[] keys, CancellationToken cancellationToken)
        {
            var categoryId = keys[1];

            await botClient.SendTextMessageAsync(
                chatId: update.CallbackQuery.Message.Chat.Id,
                text: Localization.GetLocalizedCommand("enter-amount",user.LanguageCode),
                cancellationToken: cancellationToken);

            _state.SetState(user.TelegramId, EUserState.Amount, int.Parse(categoryId));
        }

        private async Task HandleTransferTypeSelectAsync(ITelegramBotClient botClient, Update update, User user, string[] keys, CancellationToken cancellationToken)
        {
            var transferType = keys[1];

            List<Category> categories = null;

            //todo transfer typega qarab filter qil
            if(transferType == "+")
            {
                categories = await _categoryRepository.GetAllAsync();
            }
            else if (transferType == "-")
            {
                categories = await _categoryRepository.GetAllAsync();
            }

            List<List<InlineKeyboardButton>> buttons = categories.Select(x => new List<InlineKeyboardButton>() { InlineKeyboardButton.WithCallbackData(x.GetLocalizedName(user.LanguageCode), $"select-category {x.Id}") }).ToList();

            await botClient.SendTextMessageAsync(
                    chatId: update.CallbackQuery.Message.Chat.Id,
                    text: Localization.GetLocalizedCommand("select", user.LanguageCode),
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

            var buttons = new List<InlineKeyboardButton>() {
                InlineKeyboardButton.WithCallbackData(
                    text: Localization.GetLocalizedCommand("income",user.LanguageCode),
                    callbackData: "select-transfer-type +"),
                InlineKeyboardButton.WithCallbackData(
                    text: Localization.GetLocalizedCommand("expense",user.LanguageCode),
                    callbackData: "select-transfer-type -")
            };

            await botClient.SendTextMessageAsync(
                chatId: update.CallbackQuery.Message.Chat.Id,
                text: Localization.GetLocalizedCommand("select", user.LanguageCode),
                replyMarkup: new InlineKeyboardMarkup(buttons),
                cancellationToken: cancellationToken);
        }
    }
}
