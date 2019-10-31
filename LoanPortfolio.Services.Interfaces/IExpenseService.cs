using System;
using System.Collections.Generic;
using LoanPortfolio.Db.Entities;
// ReSharper disable CommentTypo

namespace LoanPortfolio.Services.Interfaces
{
    public interface IExpenseService
    {
        /// <summary>
        /// Создает новый личный расход
        /// </summary>
        /// <param name="user">Пользователь к которому относится расход</param>
        /// <param name="datePayment">Дата расхода</param>
        /// <param name="sum">Сумма расхода</param>
        /// <param name="expenseCategory">Категория расхода</param>
        /// <returns>Только что созданный личный расход</returns>
        PersonalExpense AddPersonalExpense(User user, DateTime datePayment, float sum, ExpenseCategory expenseCategory);

        /// <summary>
        /// Создает новый расход по ЖКХ
        /// </summary>
        /// <param name="user">Пользователь к которому относится расход</param>
        /// <param name="datePayment">Дата расхода</param>
        /// <param name="sum">Сумма расхода</param>
        /// <param name="comment">Комментарий к расходу</param>
        /// <returns></returns>
        HCSExpense AddHCSExpense(User user, DateTime datePayment, float sum, string comment = "");

        /// <summary>
        /// Создает новый кредит
        /// </summary>
        /// <param name="user">Пользователь к которому относится кредит</param>
        /// <param name="datePayment">Дата платежа</param>
        /// <param name="loanSum">Сумма займа</param>
        /// <param name="amountDie">Сумма к погашению</param>
        /// <param name="repaymentPeriod">Срок погашения в месяцах</param>
        /// <param name="creditInstitutionName">Наименование кредитной организации</param>
        /// <param name="bankAddress">Адрес банка или банкомата в котором можно внести платеж</param>
        /// <returns></returns>
        Loan AddLoan(User user, DateTime datePayment, float loanSum, float amountDie, int repaymentPeriod,
            string creditInstitutionName, string bankAddress = "");

        /// <summary>
        /// Удаляет расход
        /// </summary>
        /// <param name="expense">Расход который необходимо удалить</param>
        void Remove(Expense expense);

        /// <summary>
        /// Получение расхода по его идентификатору
        /// </summary>
        /// <param name="id">Идентификатор расхода</param>
        /// <returns>Расход, в случае отсутствия null</returns>
        Expense GetById(int id);

        /// <summary>
        /// Получение списка всех расходов пользователя
        /// </summary>
        /// <param name="user">Пользователь, чьи расходы необходимо получить</param>
        /// <returns>Список расходов</returns>
        IEnumerable<Expense> GetAll(User user);

        /// <summary>
        /// Сохраняет изменения расхода в базе данных
        /// </summary>
        /// <param name="expense">Расход который необходимо обновить</param>
        void UpdateExpense(Expense expense);
    }
}