// ReSharper disable CommentTypo

using System.ComponentModel.DataAnnotations;

namespace LoanPortfolio.Db.Entities
{
    // ReSharper disable once InconsistentNaming
    /// <inheritdoc />
    /// <summary>
    /// Расход по ЖКХ ТР-18
    /// </summary>
    public class HCSExpense : Expense
    {
        /// <summary>
        /// Комментарий к расходу ТР-21
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Сумма рахода ТР-15
        /// </summary>
        [Required]
        public float Sum { get; set; }
    }
}