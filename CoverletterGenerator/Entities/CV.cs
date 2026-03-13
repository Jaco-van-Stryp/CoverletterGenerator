namespace CoverletterGenerator.Entities
{
    public class CV
    {
        public Guid Id { get; set; }
        public required string Content { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
