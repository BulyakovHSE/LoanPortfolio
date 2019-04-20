// ReSharper disable CommentTypo
namespace LoanPortfolio.Db.Entities
{
    /// <summary>
    /// Кредит ТР-28
    /// </summary>
    public class Loan : Expense
    {
        /// <summary>
        /// Сумма займа ТР-30
        /// </summary>
        public float LoanSum { get; set; }

        /// <summary>
        /// Сумма к погашению ТР-31
        /// </summary>
        public float AmountDie { get; set; }

        /// <summary>
        /// Срок погашения в месяцах ТР-32
        /// </summary>
        public int RepaymentPeriod { get; set; }

        /// <summary>
        /// Погашен ли кредит ТР-35
        /// </summary>
        public bool IsRepaid { get; set; }
    }
}