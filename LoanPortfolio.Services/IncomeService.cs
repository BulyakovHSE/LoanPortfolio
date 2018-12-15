using System;
using System.Collections.Generic;
using LoanPortfolio.Db.Entities;
using LoanPortfolio.Services.Interfaces;

namespace LoanPortfolio.Services
{
    public class IncomeService : IIncomeService
    {
        public RegularIncome AddRegularIncome(User user, string incomeSource, float sum, DateTime datePrepaidExpense,
            float prepaidExpanse, DateTime dateSalary, float salary)
        {
            throw new NotImplementedException();
        }

        public PeriodicIncome AddPeriodicIncome(User user, string incomeSource, float sum, DateTime dateIncome)
        {
            throw new NotImplementedException();
        }

        public void Remove(Income income)
        {
            throw new NotImplementedException();
        }

        public Income GetById(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Income> GetAll(User user)
        {
            throw new NotImplementedException();
        }

        public void ChangeDatePrepaidExpanse(RegularIncome income, DateTime datePrepaidExpanse)
        {
            throw new NotImplementedException();
        }

        public void ChangePrepaidExpanse(RegularIncome income, float prepaidExpanse)
        {
            throw new NotImplementedException();
        }

        public void ChangeDateSalary(RegularIncome income, DateTime dateSalary)
        {
            throw new NotImplementedException();
        }

        public void ChangeSalary(RegularIncome income, float salary)
        {
            throw new NotImplementedException();
        }

        public void ChangeIncomeSource(Income income, string incomeSource)
        {
            throw new NotImplementedException();
        }

        public void ChangeDateIncome(PeriodicIncome income, DateTime dateIncome)
        {
            throw new NotImplementedException();
        }

        public void ChangeSum(PeriodicIncome income, float sum)
        {
            throw new NotImplementedException();
        }
    }
}