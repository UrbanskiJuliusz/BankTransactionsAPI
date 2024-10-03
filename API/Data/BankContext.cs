using Microsoft.EntityFrameworkCore;
using BankTransferApi.Models;

namespace BankTransferApi.Data
{
    public class BankContext : DbContext
    {
        public BankContext(DbContextOptions<BankContext> options) : base(options) {}

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.FromAccount)
                .WithMany(a => a.FromTransactions)
                .HasForeignKey(t => t.FromAccountId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.ToAccount)
                .WithMany(a => a.ToTransactions)
                .HasForeignKey(t => t.ToAccountId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Account>().HasData(
                new Account { Id = 1, AccountNumber = "123456", Balance = 1000 },
                new Account { Id = 2, AccountNumber = "654321", Balance = 500 }
            );
        }
    }
}