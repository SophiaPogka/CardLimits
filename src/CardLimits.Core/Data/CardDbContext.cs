using CardLimits.Core.Model;
using Microsoft.EntityFrameworkCore;

namespace CardLimits.Core.Data
{
    public class CardDbContext : DbContext
    {
        string connectionstring = "Server=localhost;Database=CardLimits;User Id=sa;Password=admin!@#123;";

        public CardDbContext(
             DbContextOptions options) : base(options)
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlServer(connectionstring);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Card>()
                .ToTable("Card");

            modelBuilder.Entity<Limit>()
                .ToTable("Limit");
        }
    }
}
