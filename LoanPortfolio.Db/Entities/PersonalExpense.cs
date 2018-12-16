using System.ComponentModel.DataAnnotations;

namespace LoanPortfolio.Db.Entities
{
    public class PersonalExpense : Expense
    {
        [Required]
        public string ExpenseCategory { get; set; }
    }
}