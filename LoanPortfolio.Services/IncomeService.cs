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
            var income = _incomeRepository.Add(new RegularIncome
            {
                UserId = user.Id,
                IncomeSource = incomeSource,
                DatePrepaidExpanse = datePrepaidExpense, DateSalary = dateSalary, PrepaidExpanse = prepaidExpanse,
                Salary = salary
            });
            return (RegularIncome)income;
        }

        public PeriodicIncome AddPeriodicIncome(User user, string incomeSource, float sum, DateTime dateIncome)
        {
            var income = _incomeRepository.Add(new PeriodicIncome
            {
                UserId = user.Id, IncomeSource = incomeSource, Sum = sum, DateIncome = dateIncome
            });
            return (PeriodicIncome)income;
        }

        public void Remove(Income income)
        {
            _incomeRepository.Remove(income);
        }

        public Income GetById(int id)
        {
            return _incomeRepository.All().SingleOrDefault(x => x.Id == id);
        }

        public IEnumerable<Income> GetAll(User user)
        {
            return _incomeRepository.All().Where(x=>x.UserId == user.Id);
        }

        public void UpdateIncome(Income income)
        {
            _incomeRepository.Update(income);
        }
    }
}