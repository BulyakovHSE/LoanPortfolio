// ReSharper disable CommentTypo

using System.ComponentModel.DataAnnotations;

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
        /// Наименование кредитной организации ТР-14, 29
        /// </summary>
        [Required]
        public string CreditInstitutionName { get; set; }

        /// <summary>
        /// Платеж по кредиту
        /// </summary>
        [Required]
        public float Sum
        {
            set => value = 0;
            get => AmountDie / RepaymentPeriod;
        }

        /// <summary>
        /// Погашен ли кредит ТР-35
        /// </summary>
        public bool IsRepaid { get; set; }


        /// <summary>
        /// Адрес банка или банкомата для внесения платежа ТР-16
        /// </summary>
        [Required]
        public string BankAddress { get; set; }
    }
}