using Microsoft.EntityFrameworkCore;
using PebaFinance.Domain.Models;

namespace PebaFinance.Infrastructure.Data;

public class FinanceDbContext : DbContext
{
    public FinanceDbContext(DbContextOptions<FinanceDbContext> options) : base(options)
    {
    }

    public DbSet<Expense> Expense { get; set; }
    public DbSet<Income> Income { get; set; }

     protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Income>().ToTable("income");
            modelBuilder.Entity<Income>().HasKey(r => r.Id);
            modelBuilder.Entity<Income>().Property(r => r.Description).IsRequired().HasMaxLength(200);
            modelBuilder.Entity<Income>().Property(r => r.Value).IsRequired().HasPrecision(18, 2);
            modelBuilder.Entity<Income>().Property(r => r.Date).IsRequired();

            modelBuilder.Entity<Expense>().ToTable("expense");
            modelBuilder.Entity<Expense>().HasKey(d => d.Id);
            modelBuilder.Entity<Expense>().Property(d => d.Description).IsRequired().HasMaxLength(200);
            modelBuilder.Entity<Expense>().Property(d => d.Value).IsRequired().HasPrecision(18, 2);
            modelBuilder.Entity<Expense>().Property(d => d.Date).IsRequired();
        }
}
