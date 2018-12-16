using System;
using System.ComponentModel.DataAnnotations;

namespace LoanPortfolio.Db.Entities
{
    public abstract class Expense : Entity
    {
        public User User { get; set; }

        public int UserId { get; set; }

        [Required]
        public DateTime DatePayment { get; set; }

        [Required]
        public float Sum { get; set; }
    }
}