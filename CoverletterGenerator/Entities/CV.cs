namespace CoverletterGenerator.Entities
{
    public class CV
    {
        public Guid Id { get; set; }
        public required string Content { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Location { get; set; }
        public string? LinkedIn { get; set; }
        public string? GitHub { get; set; }
        public string? Website { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
