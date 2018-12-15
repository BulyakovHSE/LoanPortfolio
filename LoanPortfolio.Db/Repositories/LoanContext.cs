using LoanPortfolio.Db.Entities;
using System.Data.Entity;

namespace LoanPortfolio.Db.Repositories
{
    public class LoanContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Income> Incomes { get; set; }

        public DbSet<Expense> Expenses { get; set; }

        public LoanContext() : base("LoanContext")
        {
            System.Data.Entity.Database.SetInitializer(new CreateDatabaseIfNotExists<LoanContext>());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //todo: build model
            base.OnModelCreating(modelBuilder);
        }
    }
}