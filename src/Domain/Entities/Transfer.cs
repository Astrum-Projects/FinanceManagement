using Domain.Comman;

namespace Domain.Entities
{
    public class Transfer : Auditable, IDeletable
    {
        public decimal Amount { get; set; }
        public string Comment { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public bool IsDeleted { get; set; }
    }
}
