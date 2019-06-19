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

        public PersonalExpense AddPersonalExpense(User user, DateTime datePayment, float sum, ExpenseCategory expenseCategory)
        {
            var expense = _expenseRepository.Add(new PersonalExpense
            { UserId = user.Id, DatePayment = datePayment, Sum = sum, ExpenseCategory = expenseCategory});
            return (PersonalExpense)expense;
        }

        public HCSExpense AddHCSExpense(User user, DateTime datePayment, float sum, string comment = "")
        {
            var expense = _expenseRepository.Add(new HCSExpense
            { UserId = user.Id, DatePayment = datePayment, Sum = sum, Comment = comment });
            return (HCSExpense)expense;
        }

        public Loan AddLoan(User user, DateTime datePayment, float loanSum, float amountDie, int repaymentPeriod,
            string creditInstitutionName, string bankAddress = "")
        {
            var expense = _expenseRepository.Add(new Loan
            {
                UserId = user.Id, DatePayment = datePayment, LoanSum = loanSum, AmountDie = amountDie,
                RepaymentPeriod = repaymentPeriod, BankAddress = bankAddress,
                CreditInstitutionName = creditInstitutionName
            });
            return (Loan)expense;
        }

        public void Remove(Expense expense)
        {
            _expenseRepository.Remove(expense);
        }

        public Expense GetById(int id)
        {
            return _expenseRepository.All().SingleOrDefault(x => x.Id == id);
        }

        public IEnumerable<Expense> GetAll(User user)
        {
            return _expenseRepository.All().Where(x => x.UserId == user.Id);
        }

        public void UpdateExpense(Expense expense)
        {
            _expenseRepository.Update(expense);
        }
    }
}