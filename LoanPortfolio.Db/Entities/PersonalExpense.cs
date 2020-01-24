using System.ComponentModel.DataAnnotations;
// ReSharper disable CommentTypo

namespace LoanPortfolio.Db.Entities
{
    /// <inheritdoc />
    /// <summary>
    /// Личный расход ТР-24
    /// </summary>
    public class PersonalExpense : Expense
    {
        /// <summary>
        /// Категория расхода ТР-25
        /// </summary>
        [Required]
        public Category ExpenseCategory { get; set; }

        /// <summary>
        /// Сумма рахода ТР-15
        /// </summary>
        [Required]
        public float Sum { get; set; }
    }
}