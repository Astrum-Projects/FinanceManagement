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

namespace Application.Services.BotServices
{
    public partial class UpdateHandlerService
    {
        public async Task HandleCallbackQueryAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var callbackData = update.CallbackQuery.Data;
            var keys = callbackData.Split(' ');

            var callbackHandler = keys[0] switch
            {
                "lan" => HandleLanguageSelectAsync(botClient, update, keys, cancellationToken)
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

        private async Task HandleLanguageSelectAsync(ITelegramBotClient botClient, Update update, string[] keys, CancellationToken cancellationToken)
        {
            User user = await _userRepository.GetUserById(telegramId: update.CallbackQuery.From.Id);
            
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
        }
    }
}
