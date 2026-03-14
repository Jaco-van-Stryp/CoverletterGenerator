using Microsoft.EntityFrameworkCore;

namespace CoverletterGenerator.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Entities.User> Users { get; set; } = null!;
        public DbSet<Entities.CV> CVs { get; set; } = null!;
        public DbSet<Entities.Jobs> Jobs { get; set; } = null!;
        public DbSet<Entities.Coverletters> Coverletters { get; set; } = null!;
        public DbSet<Entities.AiTokenUsage> AiTokenUsages { get; set; } = null!;
    }
}
