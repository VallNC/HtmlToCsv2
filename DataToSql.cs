global using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace HtmlToCsv2
{
    public class DataToSql : DbContext
    {
        private readonly IConfiguration _config;


        public DbSet<Candidate> candidates => Set<Candidate>();
        public DbSet<Area> areas => Set<Area>();
        public DbSet<Person> people => Set<Person>();
         public DataToSql()  {
         
         }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
       
    {
        optionsBuilder.UseSqlServer(@"Server=.\SqlExpress; DataBase=HtmlToCsvDB; Trusted_Connection = true; TrustServerCertificate = true;");
        //  @"Server=.\\SqlExpress; DataBase=HtmlToCsvDB; Trusted_Connection = true; TrustServerCertificate = true;"
    }
      protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Candidate>()
            .OwnsMany(c => c.PersonNames);

        modelBuilder.Entity<Candidate>()
            .HasOne(c => c._Area)
            .WithMany()
            .HasForeignKey("AreaId");
        }   
    }
}