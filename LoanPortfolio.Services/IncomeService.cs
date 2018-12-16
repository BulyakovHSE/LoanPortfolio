using System;
using System.Collections.Generic;
using System.Linq;
using LoanPortfolio.Db.Entities;
using LoanPortfolio.Db.Interfaces;
using LoanPortfolio.Services.Interfaces;

namespace LoanPortfolio.Services
{
    public class IncomeService : IIncomeService
    {
        private readonly IRepository<Income> _incomeRepository;

        public IncomeService(IRepository<Income> incomeRepository)
        {
            _incomeRepository = incomeRepository;
        }

        public RegularIncome AddRegularIncome(User user, string incomeSource, DateTime datePrepaidExpense,
            float prepaidExpanse, DateTime dateSalary, float salary)
        {
            var id = _incomeRepository.Add(new RegularIncome
            {
                User = user, IncomeSource = incomeSource,
                DatePrepaidExpanse = datePrepaidExpense, DateSalary = dateSalary, PrepaidExpanse = prepaidExpanse,
                Salary = salary
            });
            return (RegularIncome)_incomeRepository.All().Single(x => x.Id == id);
        }

        public PeriodicIncome AddPeriodicIncome(User user, string incomeSource, float sum, DateTime dateIncome)
        {
            var id = _incomeRepository.Add(new PeriodicIncome
            {
                User = user, IncomeSource = incomeSource, Sum = sum, DateIncome = dateIncome
            });
            return (PeriodicIncome)_incomeRepository.All().Single(x => x.Id == id);
        }

        public void Remove(Income income)
        {
            _incomeRepository.Remove(income);
        }

        public Income GetById(int id)
        {
            return _incomeRepository.All().First(x => x.Id == id);
        }

        public IEnumerable<Income> GetAll(User user)
        {
            return _incomeRepository.All().Where(x=>x.UserId == user.Id);
        }

        public void ChangeDatePrepaidExpanse(RegularIncome income, DateTime datePrepaidExpanse)
        {
            income.DatePrepaidExpanse = datePrepaidExpanse;
            _incomeRepository.Update(income);
        }

        public void ChangePrepaidExpanse(RegularIncome income, float prepaidExpanse)
        {
            income.PrepaidExpanse = prepaidExpanse;
            _incomeRepository.Update(income);
        }

        public void ChangeDateSalary(RegularIncome income, DateTime dateSalary)
        {
            income.DateSalary = dateSalary;
            _incomeRepository.Update(income);
        }

        public void ChangeSalary(RegularIncome income, float salary)
        {
            income.Salary = salary;
            _incomeRepository.Update(income);
        }

        public void ChangeIncomeSource(Income income, string incomeSource)
        {
            income.IncomeSource = incomeSource;
            _incomeRepository.Update(income);
        }

        public void ChangeDateIncome(PeriodicIncome income, DateTime dateIncome)
        {
            income.DateIncome = dateIncome;
            _incomeRepository.Update(income);
        }

        public void ChangeSum(PeriodicIncome income, float sum)
        {
            income.Sum = sum;
            _incomeRepository.Update(income);
        }
    }
}