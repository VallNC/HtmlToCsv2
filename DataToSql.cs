global using Microsoft.EntityFrameworkCore;

namespace HtmlToCsv2
{
    public class DataToSql : DbContext
    {
        public DbSet<Candidate> candidates => Set<Candidate>();
        public DbSet<Area> areas => Set<Area>();
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            @"Server=.\\SqlExpress; DataBase=HtmlToCsvDB; Trusted_Connection = true; TrustServerCertificate = true;");
    }
      protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }   
    }
}