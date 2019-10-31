using System;
using System.ComponentModel.DataAnnotations;
// ReSharper disable CommentTypo

namespace LoanPortfolio.Db.Entities
{
    /// <inheritdoc />
    /// <summary>
    /// Непостоянный доход ТР-7
    /// </summary>
    public class PeriodicIncome : Income
    {
        /// <summary>
        /// Дата зачисления дохода
        /// </summary>
        [Required]
        public DateTime DateIncome { get; set; }

        /// <summary>
        /// Сумма дохода ТР-9
        /// </summary>
        [Required]
        public float Sum { get; set; }
    }
}