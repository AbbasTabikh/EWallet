using EWallet.Entities;
using Microsoft.EntityFrameworkCore;

namespace EWallet.Data
{
    public class DataContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Budget> Budgets { get; set; }
        public DbSet<Expense> Expenses { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //user entity
            modelBuilder.Entity<User>().HasIndex(x => x.Username).IsUnique();
            modelBuilder.Entity<User>().Property(x => x.Password).IsRequired();
            modelBuilder.Entity<User>().Property(x => x.Username).IsRequired();

            //budget entity
            modelBuilder.Entity<Budget>().Property(x => x.Total).HasPrecision(12, 2); //9,999,999,999.99

            //expense entity
            modelBuilder.Entity<Expense>().Property(x => x.Price).HasPrecision(12, 2);
            modelBuilder.Entity<Expense>().Property(x => x.Name).IsRequired();
            modelBuilder.Entity<Expense>().Property(x => x.Category).HasConversion<byte>();

            modelBuilder.Entity<User>()
                        .HasMany(x => x.Budgets)
                        .WithOne(x => x.User)
                        .HasForeignKey(x => x.UserID)
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

            modelBuilder.Entity<Budget>()
                        .HasMany(x => x.Expenses)
                        .WithOne(x => x.Budget)
                        .HasForeignKey(x => x.BudgetID)
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

            
        }
    }
}
