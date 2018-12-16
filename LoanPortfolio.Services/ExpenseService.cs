using System;
using System.Collections.Generic;
using System.Linq;
using LoanPortfolio.Db.Entities;
using LoanPortfolio.Db.Interfaces;
using LoanPortfolio.Services.Interfaces;

namespace LoanPortfolio.Services
{
    public class ExpenseService : IExpenseService
    {
        private readonly IRepository<Expense> _expenseRepository;

        public ExpenseService(IRepository<Expense> expenseRepository)
        {
            _expenseRepository = expenseRepository;
        }

        public PersonalExpense AddPersonalExpense(User user, DateTime datePayment, float sum, string expenseCategory)
        {
            var id = _expenseRepository.Add(new PersonalExpense
            { User = user, DatePayment = datePayment, Sum = sum, ExpenseCategory = expenseCategory });
            return (PersonalExpense)_expenseRepository.All().Single(x => x.Id == id);
        }

        public HCSExpense AddHCSExpense(User user, DateTime datePayment, float sum)
        {
            var id = _expenseRepository.Add(new HCSExpense
            { User = user, DatePayment = datePayment, Sum = sum });
            return (HCSExpense)_expenseRepository.All().Single(x => x.Id == id);
        }

        public void Remove(Expense expense)
        {
            _expenseRepository.Remove(expense);
        }

        public Expense GetById(int id)
        {
            return _expenseRepository.All().Single(x => x.Id == id);
        }

        public IEnumerable<Expense> GetAll(User user)
        {
            return _expenseRepository.All().Where(x => x.UserId == user.Id);
        }

        public void ChangeDatePayment(Expense expense, DateTime newDate)
        {
            expense.DatePayment = newDate;
            _expenseRepository.Update(expense);
        }

        public void ChangeSum(Expense expense, float sum)
        {
            expense.Sum = sum;
            _expenseRepository.Update(expense);
        }

        public void ChangeExpanseCategory(PersonalExpense expense, string category)
        {
            expense.ExpenseCategory = category;
            _expenseRepository.Update(expense);
        }
    }
}