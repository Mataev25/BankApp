using Microsoft.EntityFrameworkCore;
using BankApp.Models;

namespace BankApp.Data {
    public class AppDbContext : DbContext {
        public DbSet<User> Users {get; set;}
        public DbSet<Card> Cards {get; set;}
        public DbSet<Account> Accounts {get; set;}
        public DbSet<Transaction> Transactions {get; set;}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseSqlServer("Server=localhost;Database=BankApp;Trusted_Connection=True; TrustServerCertificate=True");

        }
    }
}