using Application.Helper;
using Infrastructure.Repositories.Categories;
using Infrastructure.Repositories.Transfers;
using Infrastructure.Repositories.Users;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Application.Services.BotServices
{
    public partial class UpdateHandlerService
    {
        private readonly IUserRepository _userRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ITransferRepository _transferRepository;
        private readonly State _state;
        public UpdateHandlerService()
        {
            _userRepository = new UserRepository();
            _categoryRepository = new CategoryRepository();
            _transferRepository = new TransferRepository();
            _state = new State();
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
