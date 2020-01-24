using System;
using System.Collections.Generic;
using LoanPortfolio.Db.Entities;

namespace LoanPortfolio.Services.Interfaces
{
    public interface ILoanService
    {
        /// <summary>
        /// Добавление нового кредита
        /// </summary>
        /// <param name="user">Пользователь, которому кредит добавляется</param>
        /// <param name="loanSum">Сумма займа</param>
        /// <param name="clearanceDate">Дата, когда кредит был оформлен</param>
        /// <param name="amountDie">Сумма к погашению</param>
        /// <param name="repaymentPeriod">Срок погашения в месяцах</param>
        /// <param name="creditInstitutionName">Наименование кредитной организации</param>
        /// <param name="bankAddress">Адрес банка или банкомата для внесения платежа</param>
        /// <returns></returns>
        Loan AddLoan(User user, float loanSum, DateTime clearanceDate, float amountDie, int repaymentPeriod,
            string creditInstitutionName, string bankAddress = "");

        /// <summary>
        /// Удаление кредита
        /// </summary>
        /// <param name="loan">Кредит, который необходимо удалить</param>
        void Remove(Loan loan);

        /// <summary>
        /// Получение кредита по его идентификатору
        /// </summary>
        /// <param name="id">Идентификатор кредита</param>
        /// <returns></returns>
        Loan GetById(int id);

        /// <summary>
        /// Получение всех кредитов пользователя
        /// </summary>
        /// <param name="user">Пользователь чьи кредиты необходимо получить</param>
        /// <returns></returns>
        IEnumerable<Loan> GetAll(User user);

        /// <summary>
        /// Сохранение изменений кредита
        /// </summary>
        /// <param name="loan">Кредит, чьи изменения необходимо сохранить</param>
        void UpdateLoan(Loan loan);
    }
}