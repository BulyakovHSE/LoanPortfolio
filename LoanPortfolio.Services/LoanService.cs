using System;
using System.Collections.Generic;
using System.Linq;
using LoanPortfolio.Db.Entities;
using LoanPortfolio.Db.Interfaces;
using LoanPortfolio.Services.Interfaces;

namespace LoanPortfolio.Services
{
    public class LoanService : ILoanService
    {
        private readonly IRepository<Loan> _loanRepository;
        private readonly IRepository<Expense> _expenseRepository;

        public LoanService(IRepository<Loan> loanRepository, IRepository<Expense> expenseRepository)
        {
            _loanRepository = loanRepository;
            _expenseRepository = expenseRepository;
        }

        public Loan AddLoan(User user, float loanSum, DateTime clearanceDate, float amountDie, int repaymentPeriod, string creditInstitutionName,
            string bankAddress = "")
        {
            var loan = new Loan
            {
                UserId = user.Id,
                LoanSum = loanSum,
                AmountDie = amountDie,
                BankAddress = bankAddress,
                CreditInstitutionName = creditInstitutionName,
                RepaymentPeriod = repaymentPeriod,
                ClearanceDate = clearanceDate
            };
            var payments = new Dictionary<DateTime, float>();
            LoanPayment payment = null;
            var date = loan.ClearanceDate.AddMonths(1);
            for (int i = 0; i < loan.RepaymentPeriod; i++)
            {
                var sum = loan.AmountDie / loan.RepaymentPeriod;
                payments.Add(date, sum);
                if (DateTime.Now.Month == date.Month && DateTime.Now.Year == date.Year)
                    payment = new LoanPayment{BankAddress = loan.BankAddress, CreditInstitutionName = loan.CreditInstitutionName, UserId = loan.UserId, DatePayment = date, Sum = sum};
                date = date.AddMonths(1);
            }

            loan.PaymentsSchedule = payments;
            loan = _loanRepository.Add(loan);
            if (payment != null)
            {
                payment.LoanId = loan.Id;
                _expenseRepository.Add(payment);
                //loan.Payments = new List<LoanPayment>(new []{payment});
                //_loanRepository.Update(loan);
            }
            return loan;
        }

        public void Remove(Loan loan)
        {
            _loanRepository.Remove(loan);
        }

        public Loan GetById(int id)
        {
            return _loanRepository.All().SingleOrDefault(x => x.Id == id);
        }

        public IEnumerable<Loan> GetAll(User user)
        {
            return _loanRepository.All().Where(x => x.UserId == user.Id);
        }

        public void UpdateLoan(Loan loan)
        {
            _loanRepository.Update(loan);
        }
    }
}