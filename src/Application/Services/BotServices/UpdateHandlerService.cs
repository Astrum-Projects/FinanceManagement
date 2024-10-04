using Infrastructure.Repositories.Users;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Application.Services.BotServices
{
    public partial class UpdateHandlerService
    {
        private readonly IUserRepository _userRepository;
        public UpdateHandlerService()
        {
            _userRepository = new UserRepository();
        }

        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var handler = update.Type switch
            {
                UpdateType.Message => HandleMessageAsync(botClient, update, cancellationToken),
                UpdateType.CallbackQuery => HandleCallbackQueryAsync(botClient, update, cancellationToken),
                _ => throw new NotImplementedException()
            };

            try
            {
                await handler;
            }
            catch(Exception ex)
            {
                await HandleErrorAsync(botClient, ex, cancellationToken);
            }
        }


        public async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            Console.WriteLine(exception.Message);
        }
    }
}
