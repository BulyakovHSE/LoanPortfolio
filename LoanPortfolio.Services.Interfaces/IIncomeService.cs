using System;
using System.Collections.Generic;
using LoanPortfolio.Db.Entities;
// ReSharper disable CommentTypo

namespace LoanPortfolio.Services.Interfaces
{
    public interface IIncomeService
    {
        /// <summary>
        /// Создает новый постоянный доход
        /// </summary>
        /// <param name="user">Пользователь к которому относится доход</param>
        /// <param name="incomeSource">Источник дохода</param>
        /// <param name="datePrepaidExpense">Дата аванса</param>
        /// <param name="prepaidExpanse">Сумма аванса</param>
        /// <param name="dateSalary">Дата оклада</param>
        /// <param name="salary">Сумма оклада</param>
        /// <returns>Только что созданный постоянный доход</returns>
        RegularIncome AddRegularIncome(User user, string incomeSource, DateTime datePrepaidExpense,
            float prepaidExpanse, DateTime dateSalary, float salary);

        /// <summary>
        /// Создает новый нерегулярный доход
        /// </summary>
        /// <param name="user">Пользователь к которому относится доход</param>
        /// <param name="incomeSource">Источник дохода</param>
        /// <param name="sum">Сумма дохода</param>
        /// <param name="dateIncome">Дата получения дохода</param>
        /// <returns>Только что созданный нерегулярный доход</returns>
        PeriodicIncome AddPeriodicIncome(User user, string incomeSource, float sum, DateTime dateIncome);

        /// <summary>
        /// Удаляет доход
        /// </summary>
        /// <param name="income">Доход который нужно удалить</param>
        void Remove(Income income);

        /// <summary>
        /// Получение дохода по его идентификатору
        /// </summary>
        /// <param name="id">Идентификатор дохода</param>
        /// <returns>Доход, в случае отсутствия null</returns>
        Income GetById(int id);

        /// <summary>
        /// Получение списка всех доходов пользователя
        /// </summary>
        /// <param name="user">Пользователь, чьи доходы нужно получить</param>
        /// <returns>Спосок доходов</returns>
        IEnumerable<Income> GetAll(User user);

        /// <summary>
        /// Сохраняет изменения дохода в базе данных
        /// </summary>
        /// <param name="income">Доход который необходимо обновить</param>
        void UpdateIncome(Income income);
    }
}