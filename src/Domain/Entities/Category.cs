using Domain.Comman;

namespace Domain.Entities
{
    public class Category : BaseEntity, ILocalizedName, IDeletable
    {
        public bool IsIncome { get; set; }

        public string NameUz { get; set; }
        public string NameRu { get; set; }
        public string NameEn { get; set; }
        public bool IsDeleted { get ; set ; }

        public int? UserId { get; set; }
        public User User { get; set; }
    }
}
