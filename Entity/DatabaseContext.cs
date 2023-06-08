using Microsoft.EntityFrameworkCore;
using System.Xml;

namespace GorevYoneticisiProjesi.Entity
{
    public class DatabaseContext : DbContext
    {

        public DbSet<Task> Tasks { get; set; }
        public DbSet<Users> Users { get; set; }
   


        public DatabaseContext(DbContextOptions<DatabaseContext> options) 
            : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            optionsBuilder.UseSqlServer("Data Source=DESKTOP-H6C6A71\\MSSQLSERVERYENI;Initial Catalog=TaskDB;Integrated Security=SSPI;TrustServerCertificate=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Task>()
                     .HasKey(e => e.Id);
            modelBuilder.Entity<Users>()
                   .HasKey(e => e.Id);
        }



    }
}
