using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LoanPortfolio.Db.Entities
{
    /// <summary>
    /// Кредит ТР-28
    /// </summary>
    public class Loan : Entity
    {
        /// <summary>
        /// Пользователь к которому относится кредит
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Идентификатор пользователя к которому относится кредит
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Дата, когда кредит был оформлен
        /// </summary>
        public DateTime ClearanceDate { get; set; }

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
        /// Наименование кредитной организации ТР-29
        /// </summary>
        [Required]
        public string CreditInstitutionName { get; set; }

        /// <summary>
        /// Адрес банка или банкомата для внесения платежа ТР-16
        /// Используется только для автозаполнения данного поля в платеже
        /// </summary>
        [Required]
        public string BankAddress { get; set; }

        /// <summary>
        /// Ежемесячный платеж, указанный банком
        /// </summary>
        public float BankSpecifiedPayment { get; set; }

        /// <summary>
        /// Погашен ли кредит ТР-35
        /// </summary>
        public bool IsRepaid { get; set; }

        /// <summary>
        /// График платежей
        /// </summary>
        [Required]
        public Dictionary<DateTime, float> PaymentsSchedule { get; set; }

        /// <summary>
        /// Список платежей по кредиту
        /// </summary>
        public IList<LoanPayment> Payments { get; set; }
    }
}