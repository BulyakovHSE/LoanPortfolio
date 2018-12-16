using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LoanPortfolio.Db.Entities
{
    public class User : Entity
    {
        [Required]
        [Key]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public IList<Income> Incomes { get; set; }

        public IList<Expense> Expenses { get; set; }
    }
}