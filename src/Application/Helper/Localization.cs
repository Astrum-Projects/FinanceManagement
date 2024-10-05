using System.Text.Json;

namespace Application.Helper
{
    public class Localization
    {
        public static string GetLocalizedCommand(string command, string languageCode)
        {
            var jsonfilePath = @"..\..\..\..\Application\Helper\localization.json"; // Path to localization file
            var jsonString = File.ReadAllText(jsonfilePath);
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
    }

    public class LocalizationCommand
    {
        public string Command { get; set; }
        public string Uz { get; set; }
        public string Ru { get; set; }
        public string En { get; set; }
    }
}
