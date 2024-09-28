using Domain.Comman;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class User : Auditable, IDeletable
    {
        public long TelegramId { get; set; }

        public string FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Username { get; set; }
        public string? LanguageCode { get; set; }
        public bool IsDeleted { get ; set ; }
    }
}
