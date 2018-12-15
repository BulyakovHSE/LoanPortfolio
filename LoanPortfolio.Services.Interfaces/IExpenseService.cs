using System;
using System.Collections.Generic;
using LoanPortfolio.Db.Entities;

namespace LoanPortfolio.Services.Interfaces
{
    public interface IExpenseService
    {
        PersonalExpense AddPersonalExpense(User user, DateTime datePayment, float sum, string expenseCategory);

        HCSExpense AddHCSExpense(User user, DateTime datePayment, float sum);

        void Remove(Expense expense);

        Expense GetById(int id);

        IEnumerable<Expense> GetAll(User user);

        void ChangeDatePayment(Expense expense, DateTime newDate);

        void ChangeSum(Expense expense, float sum);

        void ChangeExpanseCategory(PersonalExpense expense, string category);
    }
}