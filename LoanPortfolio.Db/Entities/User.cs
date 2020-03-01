using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
// ReSharper disable CommentTypo

namespace LoanPortfolio.Db.Entities
{
    /// <summary>
    /// Пользователь
    /// </summary>
    public class User : Entity
    {
        /// <summary>
        /// Электронная почта пользователя, Логин
        /// </summary>
        [Required]
        [Index(IsUnique = true)]
        [MaxLength(255)]
        public string Email { get; set; }

        /// <summary>
        /// Пароль пользователя
        /// </summary>
        [Required]
        public string Password { get; set; }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        [Required]
        public string FirstName { get; set; }

        /// <summary>
        /// Фамилия пользователя
        /// </summary>
        [Required]
        public string LastName { get; set; }

        /// <summary>
        /// Список доходов пользователя
        /// </summary>
        public IList<Income> Incomes { get; set; }

        /// <summary>
        /// Список расходов пользователя
        /// </summary>
        public IList<Expense> Expenses { get; set; }

        /// <summary>
        /// Список кредитов пользователя
        /// </summary>
        public IList<Loan> Loans { get; set; }

        /// <summary>
        /// Список категорий пользователя
        /// </summary>
        public IList<Category> Categories { get; set; }
    }
}