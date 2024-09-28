namespace Domain.Comman
{
    public abstract class Auditable : BaseEntity
    {
        public DateTime CreatedAt { get; set; }
    }
}
