using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Moq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using LoanPortfolio.Db.Interfaces;
using LoanPortfolio.Db.Repositories;
using LoanPortfolio.Db.Entities;
using LoanPortfolio.Services;
using LoanPortfolio.Services.Interfaces;
using System.Data.Entity;

namespace CreditPortfolioUnitTests
{
    [TestClass]
    public class LoanPaymentTests
    {
        private LoanService loanService;
        private ExpenseService expenseService;

        private User _user;
        private float _loanSum;
        private DateTime _clearanceDate;
        private float _amountDie;
        private int _repaymentPeriod;
        private string _institutionName;
        private string _bankAddress;
        private float _paymentSum;

        private Loan loan;
        private LoanPayment payment;

        [TestInitialize]
        public void TestInitialize()
        {
            var mockLoanRepository = new Mock<IRepository<Loan>>();
            var mockExpenseRepository = new Mock<IRepository<Expense>>();

            _user = new User
            {
                Id = 0,
                Email = "222@mail.ru",
                Password = "12345",
                FirstName = "Test",
                LastName = "Yestovish",
                Incomes = new List<Income>(),
                Expenses = new List<Expense>(),
                Loans = new List<Loan>()
            };

            _loanSum = 10000;
            _clearanceDate = new DateTime(2020, 06, 05);
            _amountDie = 600;
            _repaymentPeriod = 3;
            _institutionName = "Курпсук";
            _bankAddress = "Орджен 3";

            loan = new Loan { Id = 0, UserId = _user.Id, LoanSum = _loanSum, AmountDie = _amountDie, BankAddress = _bankAddress, CreditInstitutionName = _institutionName, RepaymentPeriod = _repaymentPeriod, ClearanceDate = _clearanceDate, PaymentsSchedule = new Dictionary<DateTime, float>()};
            var sum = _amountDie / _repaymentPeriod;
            _paymentSum = sum;
            payment = new LoanPayment { BankAddress = _bankAddress, CreditInstitutionName = _institutionName, UserId = _user.Id, DatePayment = _clearanceDate.AddMonths(1), Sum = sum, LoanId = loan.Id};

            mockExpenseRepository.Setup(exp => exp.Add(It.IsAny<LoanPayment>())).Returns(payment);
            //mockExpenseRepository.Setup(exp => exp.All().SingleOrDefault(x=> x.Id == 0)).Returns(payment);

            loanService = new LoanService(mockLoanRepository.Object, mockExpenseRepository.Object);
            expenseService = new ExpenseService(mockExpenseRepository.Object);
        }

        [TestMethod]
        public void GetLoanPayment()
        {
            var actual = (LoanPayment)expenseService.GetById(0);
            Assert.AreEqual(payment, actual);
        }

        [TestMethod]
        public void CheckLoanPaymentLoan()
        {
            var actual = (LoanPayment)expenseService.GetById(0);
            Assert.AreEqual(loan, actual.Loan);
        }

        [TestMethod]
        public void CheckLoanPaymentLoanId()
        {
            var actual = (LoanPayment)expenseService.GetById(0);
            Assert.AreEqual(loan.Id, payment.LoanId);
        }

        [TestMethod]
        public void CheckLoanPaymentInstitutionName()
        {
            var actual = (LoanPayment)expenseService.GetById(0);
            Assert.AreEqual(_institutionName, actual.CreditInstitutionName);
        }

        [TestMethod]
        public void CheckLoanPaymentSum()
        {
            var actual = (LoanPayment)expenseService.GetById(0);
            Assert.AreEqual(_loanSum, actual.Sum);
        }

        [TestMethod]
        public void CheckLoanPaymentBankAddress()
        {
            var actual = (LoanPayment)expenseService.GetById(0);
            Assert.AreEqual(_loanSum, actual.Sum);
        }
    }
}
