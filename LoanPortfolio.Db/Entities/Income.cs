using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
// ReSharper disable CommentTypo

namespace LoanPortfolio.Db.Entities
{
    /// <summary>
    /// Доход ТР-1, 7
    /// </summary>
    public abstract class Income : Entity
    {
        /// <summary>
        /// Пользователь к которому относится доход
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Идентификатор пользователя к которому относится доход
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Источник дохода ТР-3, 8
        /// </summary>
        [Required]
        public string IncomeSource { get; set; }
    }
}