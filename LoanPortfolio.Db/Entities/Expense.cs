using System;

namespace LoanPortfolio.Db.Entities
{
    public abstract class Expense : Entity
    {
        public User User { get; set; }

        public int UserId => User?.Id ?? -1;

        public DateTime DatePayment { get; set; }

        public float Sum { get; set; }
    }
}