using Domain.Comman;

namespace Domain.Entities
{
    public class Category : BaseEntity, ILocalizedName, IDeletable
    {
        public string NameUz { get; set; }
        public string NameRu { get; set; }
        public string NameEn { get; set; }
        public bool IsDeleted { get ; set ; }
    }
}
