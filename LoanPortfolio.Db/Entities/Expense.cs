using System;
using System.ComponentModel.DataAnnotations;
// ReSharper disable CommentTypo

namespace LoanPortfolio.Db.Entities
{
   /// <summary>
   /// Расход ТР-12
   /// </summary>
    public abstract class Expense : Entity
    {
        /// <summary>
        /// Пользователь к которому относится расход
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Идентификатор пользователя к которому относится расход
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Дата списания расхода ТР-13
        /// </summary>
        [Required]
        public DateTime DatePayment { get; set; }

        /// <summary>
        /// Адрес банка или банкомата для внесения платежа ТР-16
        /// </summary>
        [Required]
        public string BankAddress { get; set; }
    }
}