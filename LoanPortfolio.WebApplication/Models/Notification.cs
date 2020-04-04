using System;
using LoanPortfolio.Db.Entities;

namespace LoanPortfolio.WebApplication.Models
{
    public abstract class Notification
    {
        public User User { get; }

        public abstract DateTime ExecuteDateTime { get; }

        public abstract string Subject { get; }

        public abstract string Text { get; }

        public Expense Expense { get; }

        protected Notification(Expense expense, User user)
        {
            Expense = expense;
            User = user;
        }
    }
}