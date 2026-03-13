namespace CoverletterGenerator.Entities
{
    public class Jobs
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public required string Company { get; set; }
        public required string Location { get; set; }
        public required string JobDescription { get; set; }
        public DateTime DatePosted { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        public ICollection<Coverletters> Coverletters { get; set; } = new List<Coverletters>();
    }
}
