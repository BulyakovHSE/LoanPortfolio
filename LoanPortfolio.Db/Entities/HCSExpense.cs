// ReSharper disable CommentTypo
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
    }
}