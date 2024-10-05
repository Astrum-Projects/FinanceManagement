using Domain.Entities;
using Infrastructure.Repositories.Users;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using User = Domain.Entities.User;

namespace Application.Helper
{
    public static class Context
    {
        public static async Task<User> UserAsync(this Update update)
        {
            var userTelegramId = update.Type switch
            {
                UpdateType.Message => update.Message.From.Id,
                UpdateType.CallbackQuery => update.CallbackQuery.From.Id,
                _ => throw new Exception($"Unknown update type")
            };

            return await (new UserRepository()).GetUserById(userTelegramId);
        }
    }
}
