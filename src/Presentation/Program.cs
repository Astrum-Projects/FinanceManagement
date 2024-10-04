// See https://aka.ms/new-console-template for more information
using Application.Services.BotServices;
using Infrastructure;
using Telegram.Bot;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Bot ishga tushdi!");

        var botClient = new TelegramBotClient(Configuration.TelegramBotToken);

        var updateHandler = new UpdateHandlerService();

        botClient.StartReceiving(
                        updateHandler: updateHandler.HandleUpdateAsync,
                        pollingErrorHandler: updateHandler.HandleErrorAsync,
                        receiverOptions: null,
                        cancellationToken: default);


        Console.ReadLine();
    }
}