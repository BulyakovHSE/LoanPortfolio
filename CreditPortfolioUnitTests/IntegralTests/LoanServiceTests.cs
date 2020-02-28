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

namespace CreditPortfolioUnitTests.IntegralTests
{
    [TestClass]
    public class LoanServiceTests
    {
        private string _dbLoanPath;
        private string _dbExpensePath;
        private LoanContext _dbLoanContext;
        private DbContext _dbExpenseContext;
        private LoanService loanService;

        private User _user;
        private float _loanSum;
        private DateTime _clearanceDate;
        private float _amountDie;
        private int _repaymentPeriod;
        private string _creditInstitutionName;
        private string _bankAddress;

        [TestInitialize]
        public void TestInitialize()
        {
            _user = new User
            {
                Id = 1,
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
            _repaymentPeriod = 13;
            _creditInstitutionName = "Курпсук";
            _bankAddress = "Орджен 3";

            _dbLoanPath = "";
            _dbLoanContext = new LoanContext();
            _dbLoanContext.Database.CreateIfNotExists();
            _dbExpensePath = "";
            _dbExpenseContext = new DbContext(_dbExpensePath);
            _dbExpenseContext.Database.CreateIfNotExists();
            EntityFrameworkRepository<Loan> loanRepository = new EntityFrameworkRepository<Loan>(_dbLoanContext);
            EntityFrameworkRepository<Expense> expenseRepository = new EntityFrameworkRepository<Expense>(_dbExpenseContext);
            loanService = new LoanService(loanRepository, expenseRepository);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _dbLoanContext.Database.Delete();
            _dbExpenseContext.Database.Delete();
        }

        [TestMethod]
        public void AddLoanTest()
        {
            Loan expected = new Loan
            {
                UserId = _user.Id,
                LoanSum = _loanSum,
                AmountDie = _amountDie,
                BankAddress = _bankAddress,
                CreditInstitutionName = _creditInstitutionName,
                RepaymentPeriod = _repaymentPeriod,
                ClearanceDate = _clearanceDate,
                PaymentsSchedule = new Dictionary<DateTime, float>()
            };

            var date = expected.ClearanceDate.AddMonths(1);
            var sum = expected.AmountDie / expected.RepaymentPeriod;
            expected.PaymentsSchedule.Add(date, sum);

            Loan actual = loanService.AddLoan(_user, _loanSum, _clearanceDate, _amountDie, _repaymentPeriod, _creditInstitutionName, _bankAddress);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void AddPaymentTest()
        {
            Loan loan = loanService.AddLoan(_user, _loanSum, _clearanceDate, _amountDie, _repaymentPeriod, _creditInstitutionName, _bankAddress);
            var date = loan.ClearanceDate.AddMonths(1);
            var sum = loan.AmountDie / loan.RepaymentPeriod;

            LoanPayment actual = new LoanPayment {BankAddress = _bankAddress, CreditInstitutionName = _creditInstitutionName, UserId =_user.Id,  DatePayment = _clearanceDate.AddMonths(1), Sum = sum, LoanId = loan.Id };

            EntityFrameworkRepository<Expense> repository = new EntityFrameworkRepository<Expense>(_dbExpenseContext);
            LoanPayment expected = (LoanPayment)repository.All().SingleOrDefault(x => x.Id == 0);

            Assert.AreEqual(expected,actual);
        }

        [TestMethod]
        public void GetByIdTest()
        {
            Loan loan = new Loan
            {
                UserId = _user.Id,
                LoanSum = _loanSum,
                AmountDie = _amountDie,
                BankAddress = _bankAddress,
                CreditInstitutionName = _creditInstitutionName,
                RepaymentPeriod = _repaymentPeriod,
                ClearanceDate = _clearanceDate,
                PaymentsSchedule = new Dictionary<DateTime, float>()
            };

            Loan actual = loanService.GetById(0);
            Assert.AreEqual(loan, actual);
        }

        [TestMethod]
        public void GetAllTest()
        {
            Loan loan1 = loanService.AddLoan(_user, _loanSum, _clearanceDate, _amountDie, _repaymentPeriod, _creditInstitutionName, _bankAddress);
            Loan loan2 = loanService.AddLoan(_user, 100, new DateTime(2020,04,10), 1, 12, "ERF", "Подъём 10");
            Loan loan3 = loanService.AddLoan(_user, 66666, new DateTime(2020,06,29), 666, 6, "ДЕмон Инд", "Ад 666");
            List<Loan> expected = new List<Loan>() { loan1, loan2, loan3 };

            IEnumerable<Loan> actual = loanService.GetAll(_user);
            CollectionAssert.AreEqual(expected, actual.ToList());
        }

        [TestMethod]
        public void RemoveTest()
        {
            Loan loan1 = loanService.AddLoan(_user, _loanSum, _clearanceDate, _amountDie, _repaymentPeriod, _creditInstitutionName, _bankAddress);
            Loan loan2 = loanService.AddLoan(_user, 100, new DateTime(2020, 04, 10), 1, 12, "ERF", "Подъём 10");
            Loan loan3 = loanService.AddLoan(_user, 66666, new DateTime(2020, 06, 29), 666, 6, "ДЕмон Инд", "Ад 666");

            loanService.Remove(loan2);

            IEnumerable<Loan> actual = loanService.GetAll(_user);
            CollectionAssert.DoesNotContain(actual.ToList(), loan2);
        }

        [TestMethod]
        public void UpdateTest()
        {
            Loan loan = loanService.AddLoan(_user, _loanSum, _clearanceDate, _amountDie, _repaymentPeriod, _creditInstitutionName, _bankAddress);

            loan.BankAddress = "efxs 5";
            loan.CreditInstitutionName = "hnhsd";
            loanService.UpdateLoan(loan);

            Loan actual = loanService.GetById(0);
            Assert.AreEqual(loan, actual);
        }
    }
}
