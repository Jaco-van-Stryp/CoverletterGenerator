namespace CoverletterGenerator.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public ICollection<CV> CVs { get; set; } = new List<CV>();
        public ICollection<Jobs> Jobs { get; set; } = new List<Jobs>();
        public ICollection<Coverletters> Coverletters { get; set; } = new List<Coverletters>();
    }
}
