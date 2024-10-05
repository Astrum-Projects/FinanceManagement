using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot;
using Application.Helper;
using Domain.Entities;
using User = Domain.Entities.User;
using Telegram.Bot.Types.ReplyMarkups;

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
                "lan" => HandleLanguageSelectAsync(botClient, update, user, keys, cancellationToken)
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
                InlineKeyboardButton.WithCallbackData(Localization.GetLocalizedCommand(Localization.GetLocalizedCommand("income",user.LanguageCode), "transfer-type +")),
                InlineKeyboardButton.WithCallbackData(Localization.GetLocalizedCommand(Localization.GetLocalizedCommand("expense",user.LanguageCode), "transfer-type -"))
            };

            await botClient.SendTextMessageAsync(
                chatId: update.CallbackQuery.Message.Chat.Id,
                text: Localization.GetLocalizedCommand("select", user.LanguageCode),
                replyMarkup: new InlineKeyboardMarkup(buttons),
                cancellationToken: cancellationToken);
        }
    }
}
