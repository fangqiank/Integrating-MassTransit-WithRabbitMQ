namespace FomularOne.Entities
{
    public class BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime AddedDate { get; set; } = DateTime.UtcNow;
        public DateTime UpdatededDate { get; set; } = DateTime.UtcNow;
        public int Status { get; set; }
    }
}
