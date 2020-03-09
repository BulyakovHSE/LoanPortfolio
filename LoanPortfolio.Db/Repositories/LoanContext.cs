using LoanPortfolio.Db.Entities;
using System.Data.Entity;

namespace LoanPortfolio.Db.Repositories
{
    public class LoanContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Income> Incomes { get; set; }

        public DbSet<Expense> Expenses { get; set; }

        public DbSet<Loan> Loans { get; set; }

        public LoanContext() : base("LoanContext")
        {
            System.Data.Entity.Database.SetInitializer(new CreateDatabaseIfNotExists<LoanContext>());
            this.Configuration.LazyLoadingEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasMany(u => u.Incomes).WithRequired(i => i.User).HasForeignKey(i => i.UserId);
            modelBuilder.Entity<User>().HasMany(u => u.Expenses).WithRequired(e => e.User).HasForeignKey(e => e.UserId);
            modelBuilder.Entity<User>().HasMany(u => u.Loans).WithRequired(l => l.User).HasForeignKey(l => l.UserId).WillCascadeOnDelete(false);
            modelBuilder.Entity<Loan>().HasMany(x => x.Payments).WithRequired(p => p.Loan).HasForeignKey(p => p.LoanId);
            modelBuilder.Entity<User>().HasMany(u=>u.Categories).WithRequired(c=>c.User).HasForeignKey(c=>c.UserId).WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);
        }
    }
}