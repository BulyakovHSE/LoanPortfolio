using LoanPortfolio.Db.Entities;
using System.Data.Entity;

namespace LoanPortfolio.Db.Repositories
{
    public class LoanContext : DbContext
    {
        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<Income> Incomes { get; set; }

        public virtual DbSet<Expense> Expenses { get; set; }

        public virtual DbSet<Loan> Loans { get; set; }

        public virtual DbSet<Category> Categories { get; set; }

        public LoanContext() : base("LoanContext")
        {
            System.Data.Entity.Database.SetInitializer(new CreateDatabaseIfNotExists<LoanContext>());
            this.Configuration.LazyLoadingEnabled = true;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasMany(u => u.Incomes).WithRequired(i => i.User).HasForeignKey(i => i.UserId);
            modelBuilder.Entity<User>().HasMany(u => u.Expenses).WithRequired(e => e.User).HasForeignKey(e => e.UserId);
            modelBuilder.Entity<User>().HasMany(u => u.Loans).WithRequired(l => l.User).HasForeignKey(l => l.UserId).WillCascadeOnDelete(false);
            modelBuilder.Entity<Loan>().HasMany(x => x.Payments).WithRequired(p => p.Loan).HasForeignKey(p => p.LoanId);
            modelBuilder.Entity<User>().HasMany(u => u.Categories).WithRequired(c => c.User).HasForeignKey(c => c.UserId).WillCascadeOnDelete(false);
            modelBuilder.Entity<PersonalExpense>().HasRequired(p => p.ExpenseCategory);

            base.OnModelCreating(modelBuilder);
        }
    }
}