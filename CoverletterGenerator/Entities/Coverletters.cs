namespace CoverletterGenerator.Entities
{
    public class Coverletters
    {
        public Guid Id { get; set; }
        public required string Content { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        public Guid JobId { get; set; }
        public Jobs Job { get; set; } = null!;
    }
}
