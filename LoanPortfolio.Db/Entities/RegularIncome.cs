using System;
using System.ComponentModel.DataAnnotations;
// ReSharper disable CommentTypo

namespace LoanPortfolio.Db.Entities
{
    /// <inheritdoc />
    /// <summary>
    /// Постоянный доход ТР-1
    /// </summary>
    public class RegularIncome : Income
    {
        /// <summary>
        /// Дата аванса ТР-2
        /// </summary>
        [Required]
        public DateTime DatePrepaidExpanse { get; set; }

        /// <summary>
        /// Сумма аванса ТР-4
        /// </summary>
        [Required]
        public float PrepaidExpanse { get; set; }

        /// <summary>
        /// Дата оклада ТР-2
        /// </summary>
        [Required]
        public DateTime DateSalary { get; set; }

        /// <summary>
        /// Сумма оклада ТР-4
        /// </summary>
        [Required]
        public float Salary { get; set; }

        /// <summary>
        /// Общая сумма дохода за месяц
        /// </summary>
        [Required]
        public float Sum {
            set => value = 0;
            get => PrepaidExpanse + Salary; }
        }
}