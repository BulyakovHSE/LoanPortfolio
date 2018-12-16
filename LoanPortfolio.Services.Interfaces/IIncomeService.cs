using System;
using System.Collections.Generic;
using LoanPortfolio.Db.Entities;

namespace LoanPortfolio.Services.Interfaces
{
    public interface IIncomeService
    {
        RegularIncome AddRegularIncome(User user, string incomeSource, DateTime datePrepaidExpense,
            float prepaidExpanse, DateTime dateSalary, float salary);

        PeriodicIncome AddPeriodicIncome(User user, string incomeSource, float sum, DateTime dateIncome);

        void Remove(Income income);

        Income GetById(int id);

        IEnumerable<Income> GetAll(User user);

        void ChangeDatePrepaidExpanse(RegularIncome income, DateTime datePrepaidExpanse);

        void ChangePrepaidExpanse(RegularIncome income, float prepaidExpanse);

        void ChangeDateSalary(RegularIncome income, DateTime dateSalary);

        void ChangeSalary(RegularIncome income, float salary);

        void ChangeIncomeSource(Income income, string incomeSource);

        void ChangeDateIncome(PeriodicIncome income, DateTime dateIncome);

        void ChangeSum(PeriodicIncome income, float sum);
    }
}