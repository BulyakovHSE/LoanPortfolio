// ReSharper disable CommentTypo

using System.ComponentModel.DataAnnotations;

namespace LoanPortfolio.Db.Entities
{
    /// <summary>
    /// Платеж по кредиту
    /// </summary>
    public class LoanPayment : Expense
    {
        /// <summary>
        /// Кредит, к которому отнисится платеж
        /// </summary>
        public Loan Loan { get; set; }

        /// <summary>
        /// Идентификатор кредита, к которому относится платеж
        /// </summary>
        public int LoanId { get; set; }

        /// <summary>
        /// Название кредитной организации ТР-14
        /// </summary>
        [Required]
        public string CreditInstitutionName { get; set; }

        /// <summary>
        /// Сумма платежа
        /// </summary>
        public float Sum { get; set; }

        /// <summary>
        /// Адрес банка или банкомата для внесения платежа ТР-16
        /// </summary>
        [Required]
        public string BankAddress { get; set; }

        /// <summary>
        /// Выполнен ли платеж по кредиту
        /// </summary>
        public bool IsPaid { get; set; }
    }
}