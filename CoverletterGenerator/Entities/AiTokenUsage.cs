namespace CoverletterGenerator.Entities
{
    public class AiTokenUsage
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        public required string Model { get; set; }
        public long InputTokens { get; set; }
        public long OutputTokens { get; set; }
        public long CacheCreationInputTokens { get; set; }
        public long CacheReadInputTokens { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
