using Domain.Comman;
using Domain.Entities;
using Infrastructure.Repositories.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Application.Helper
{
    public static class Localization
    {
        public static string GetLocalizedCommand(string command, string languageCode)
        {
            var jsonfilePath = @"..\..\..\..\Application\Helper\localization.json"; // Path to localization file
            var jsonString = System.IO.File.ReadAllText(jsonfilePath);
            var commands = JsonSerializer.Deserialize<List<LocalizationCommand>>(jsonString);

            var localizedCommand = commands.FirstOrDefault(x => x.Command == command);

            return languageCode switch
            {
                "uz" => localizedCommand.Uz,
                "ru" => localizedCommand.Ru,
                "en" => localizedCommand.En,
                _ => command
            };
        }

        public static string GetLocalizedName(this ILocalizedName localizedName, string languageCode)
        {
            return languageCode switch
            {
                "uz" => localizedName.NameUz,
                "ru" => localizedName.NameRu,
                "en" => localizedName.NameEn,
                _ => localizedName.NameUz
            };
        }
    }

    public static class UserHelper
    {
        public static async Task<Domain.Entities.User> GetUserAsync(this Update update)
        {
            var userTelegramId = update.Type switch
            {
                Telegram.Bot.Types.Enums.UpdateType.Message => update.Message.Chat.Id,
                Telegram.Bot.Types.Enums.UpdateType.CallbackQuery => update.CallbackQuery.From.Id,
                _ => throw new Exception("Unknown update type")
            };

            return await (new UserRepository()).GetUserById(userTelegramId);
        }

    }

    public class LocalizationCommand
    {
        public string Command { get; set; }
        public string Uz { get; set; }
        public string Ru { get; set; }
        public string En { get; set; }
    }
}
