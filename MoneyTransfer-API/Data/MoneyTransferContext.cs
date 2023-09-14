using Microsoft.EntityFrameworkCore;
using MoneyTransfer_API.Helpers;
using MoneyTransfer_API.Models;

namespace MoneyTransfer_API.Data
{
    public class MoneyTransferContext : DbContext
    {
        public MoneyTransferContext(DbContextOptions<MoneyTransferContext>options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Loggs> Loggs { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(local);Database=MoneyTransferSQL;Trusted_Connection=True;MultipleActiveResultSets=True");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1, 
                    FirstName = "Admin123",
                    LastName = "Admin123",
                    UserName = "admin123",
                    Password = HashSettings.HashPassword("Admin123"),
                    Age = 30,
                    Role = "Admin",
                    Balance = 9000
                }
            );
        }

    }
}
