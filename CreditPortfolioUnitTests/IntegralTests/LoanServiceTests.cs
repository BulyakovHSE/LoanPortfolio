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
        private LoanContext _dbLoanContext;
        private LoanService loanService;
        private UserService userService;

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
            _dbLoanContext = new LoanContext();
            _dbLoanContext.Database.CreateIfNotExists();
            EntityFrameworkRepository<Loan> loanRepository = new EntityFrameworkRepository<Loan>(_dbLoanContext);
            EntityFrameworkRepository<Expense> expenseRepository = new EntityFrameworkRepository<Expense>(_dbLoanContext);
            EntityFrameworkRepository<User> userRepository = new EntityFrameworkRepository<User>(_dbLoanContext);
            loanService = new LoanService(loanRepository, expenseRepository);
            userService = new UserService(userRepository);

            if(_user == null)
            _user = userService.Add("222@mail.ru", "123456789", "Test", "Testovich");
            //_user = userService.GetById(1);

            _loanSum = 10000;
            _clearanceDate = new DateTime(2020, 06, 05);
            _amountDie = 600;
            _repaymentPeriod = 13;
            _creditInstitutionName = "Курпсук";
            _bankAddress = "Орджен 3";

        }

        [TestCleanup]
        public void TestCleanup()
        {
            _dbLoanContext.Database.Delete();
        }

        [TestMethod]
        public void AddLoanTest()
        {
            Loan expected = new Loan
            {
                UserId = _user.Id,
                User = _user,
                LoanSum = _loanSum,
                AmountDie = _amountDie,
                BankAddress = _bankAddress,
                CreditInstitutionName = _creditInstitutionName,
                RepaymentPeriod = _repaymentPeriod,
                ClearanceDate = _clearanceDate,
                PaymentsSchedule = new Dictionary<DateTime, float>()
            };

            var date = _clearanceDate;

            for (int i = 0; i < _repaymentPeriod; i++)
            {
                var sum = expected.AmountDie / expected.RepaymentPeriod;
                expected.PaymentsSchedule.Add(date,sum);
                date = date.AddMonths(1);
            }

            Loan actual = loanService.AddLoan(_user, _loanSum, _clearanceDate, _amountDie, _repaymentPeriod, _creditInstitutionName, _bankAddress);
            //Assert.AreEqual(expected, actual);
            Assert.AreEqual(expected.User, actual.User);
            Assert.AreEqual(expected.LoanSum, actual.LoanSum);
            Assert.AreEqual(expected.AmountDie, actual.AmountDie);
            Assert.AreEqual(expected.BankAddress, actual.BankAddress);
            Assert.AreEqual(expected.CreditInstitutionName, actual.CreditInstitutionName);
            Assert.AreEqual(expected.RepaymentPeriod, actual.RepaymentPeriod);
            Assert.AreEqual(expected.ClearanceDate, actual.ClearanceDate);
            //CollectionAssert.AreEquivalent(expected.PaymentsSchedule, actual.PaymentsSchedule);
        }

        [TestMethod]
        public void CheckPaymentTest()
        {
            Loan loan = loanService.AddLoan(_user, _loanSum, _clearanceDate, _amountDie, _repaymentPeriod, _creditInstitutionName, _bankAddress);

            //Loan loan = loanService.GetById(1);

            var date = loan.ClearanceDate;
            LoanPayment loanPayment = new LoanPayment();

            for (int i = 0; i < _repaymentPeriod; i++)
            {
                var sum = loan.AmountDie / loan.RepaymentPeriod;
                if (DateTime.Now.Month == date.Month && DateTime.Now.Year == date.Year)
                    loanPayment = new LoanPayment {BankAddress =_bankAddress, CreditInstitutionName = _creditInstitutionName, UserId = loan.UserId, DatePayment = date, Sum = sum };
                date = loan.ClearanceDate.AddMonths(1);
            }

            EntityFrameworkRepository<Expense> repository = new EntityFrameworkRepository<Expense>(_dbLoanContext);
            LoanPayment expected = (LoanPayment)repository.All().SingleOrDefault(x => x.Id == 1);

            Assert.AreEqual(loanPayment,expected);
        }

        [TestMethod]
        public void GetByIdTest()
        {
            //Loan loan = new Loan
            //{
            //    UserId = _user.Id,
            //    LoanSum = _loanSum,
            //    AmountDie = _amountDie,
            //    BankAddress = _bankAddress,
            //    CreditInstitutionName = _creditInstitutionName,
            //    RepaymentPeriod = _repaymentPeriod,
            //    ClearanceDate = _clearanceDate,
            //    PaymentsSchedule = new Dictionary<DateTime, float>()
            //};

            Loan loan = loanService.AddLoan(_user, _loanSum, _clearanceDate, _amountDie, _repaymentPeriod, _creditInstitutionName, _bankAddress);

            Loan actual = loanService.GetById(1);
            Assert.AreEqual(_loanSum, actual.LoanSum);
            Assert.AreEqual(_amountDie, actual.AmountDie);
            Assert.AreEqual(_bankAddress, actual.BankAddress);
            Assert.AreEqual(_creditInstitutionName, actual.CreditInstitutionName);
            Assert.AreEqual(_repaymentPeriod, actual.RepaymentPeriod);
            Assert.AreEqual(_clearanceDate, actual.ClearanceDate);
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
            Loan loan2 = loanService.AddLoan(_user, 100, new DateTime(2020,04,10), 1, 12, "ERF", "Подъём 10");
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

            Loan actual = loanService.GetById(1);
            Assert.AreEqual(loan.CreditInstitutionName, actual.CreditInstitutionName);
            Assert.AreEqual(loan.BankAddress, actual.BankAddress);
        }
    }
}
