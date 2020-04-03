using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

// ReSharper disable CommentTypo

namespace LoanPortfolio.Db.Entities
{
    /// <summary>
    /// Пользователь
    /// </summary>
    public class User : Entity, ICloneable
    {
        /// <summary>
        /// Электронная почта пользователя, Логин
        /// </summary>
        [Required]
        [Index(IsUnique = true)]
        [MaxLength(255)]
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// Пароль пользователя
        /// </summary>
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
        [DataType(DataType.Password)]
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
        public virtual IList<Income> Incomes { get; set; }

        /// <summary>
        /// Список расходов пользователя
        /// </summary>
        public virtual IList<Expense> Expenses { get; set; }

        /// <summary>
        /// Список кредитов пользователя
        /// </summary>
        public virtual IList<Loan> Loans { get; set; }

        /// <summary>
        /// Список категорий пользователя
        /// </summary>
        public virtual IList<Category> Categories { get; set; }

        public object Clone()
        {
            return new User
            {
                Categories = Categories?.ToList(), Expenses = Expenses?.ToList(), Email = Email, FirstName = FirstName,
                LastName = LastName, Incomes = Incomes?.ToList(), Loans = Loans?.ToList(), Password = Password, Id = Id
            };
        }
    }
}