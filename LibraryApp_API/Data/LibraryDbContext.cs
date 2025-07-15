using LibraryApp_API.Models;
using LibrarySystemWPF.Models;
using Microsoft.EntityFrameworkCore;
using Status = LibraryApp_API.Models.Status;

namespace LibraryApp_API.Data
{
    public class LibraryDbContext : DbContext
    {
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options) { }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<Models.Type> Types { get; set; }
        public DbSet<Language> Languages { get; set; }
        //t

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Status>().ToTable("Status");
            modelBuilder.Entity<Gender>().ToTable("Gender");
            modelBuilder.Entity<Models.Type>().ToTable("Types");
            modelBuilder.Entity<Language>().ToTable("Languages");
            modelBuilder.Entity<Client>().ToTable("Clients");

            modelBuilder.Entity<Client>()
                .HasOne(c => c.GenderRef)
                .WithMany()
                .HasForeignKey(c => c.GenderID);

            modelBuilder.Entity<Client>()
                .HasOne(c => c.StatusRef)
                .WithMany()
                .HasForeignKey(c => c.StatusID);

            modelBuilder.Entity<Client>()
                .HasOne(c => c.TypeRef)
                .WithMany()
                .HasForeignKey(c => c.TypeID);

            modelBuilder.Entity<Client>()
                .HasOne(c => c.LanguageRef)
                .WithMany()
                .HasForeignKey(c => c.LanguageID);
        }
    }
}