using System;
using System.Collections.Generic;
using LoanPortfolio.Db.Entities;
using LoanPortfolio.Services.Interfaces;

namespace LoanPortfolio.Services
{
    public class ExpenseService : IExpenseService
    {
        public PersonalExpense AddPersonalExpense(User user, DateTime datePayment, float sum, string expenseCategory)
        {
            throw new NotImplementedException();
        }

        public HCSExpense AddHCSExpense(User user, DateTime datePayment, float sum)
        {
            throw new NotImplementedException();
        }

        public void Remove(Expense expense)
        {
            throw new NotImplementedException();
        }

        public Expense GetById(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Expense> GetAll(User user)
        {
            throw new NotImplementedException();
        }

        public void ChangeDatePayment(Expense expense, DateTime newDate)
        {
            throw new NotImplementedException();
        }

        public void ChangeSum(Expense expense, float sum)
        {
            throw new NotImplementedException();
        }

        public void ChangeExpanseCategory(PersonalExpense expense, string category)
        {
            throw new NotImplementedException();
        }
    }
}