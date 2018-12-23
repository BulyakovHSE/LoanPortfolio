using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoanPortfolio.Db.Entities
{
    public abstract class Income : Entity
    {
        public User User { get; set; }

        public int UserId { get; set; }

        [Required]
        public string IncomeSource { get; set; }
    }
}