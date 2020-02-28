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
    public class LoanServiceTests
    {
        private LoanService loanService;
        private string DbConnect;

        private User user = new User
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
        private float loanSum = 10000;
        private DateTime clearanceDate = new DateTime(2020, 06, 05);
        private float amountDie = 600;
        private int repaymentPeriod = 13;
        private string creditInstitutionName = "Курпсук";
        private string bankAddress = "Орджен 3";

        [TestInitialize]
        public void TestInitialize()
        {
            //loanService = new LoanService(new EntityFrameworkRepository<Loan>(new DbContext(DbConnect)), new EntityFrameworkRepository<Expense>(new DbContext(DbConnect)));
        }

        [TestMethod]
        public void AddLoanTest()
        {
            Loan expected = new Loan
            {
                UserId = user.Id,
                LoanSum = loanSum,
                AmountDie = amountDie,
                BankAddress = bankAddress,
                CreditInstitutionName = creditInstitutionName,
                RepaymentPeriod = repaymentPeriod,
                ClearanceDate = clearanceDate,
                PaymentsSchedule = new Dictionary<DateTime, float>()
            };

            var date = expected.ClearanceDate.AddMonths(1);
            var sum = expected.AmountDie / expected.RepaymentPeriod;
            expected.PaymentsSchedule.Add(date, sum);

            LoanPayment payment = new LoanPayment { BankAddress = bankAddress, CreditInstitutionName = creditInstitutionName, UserId = user.Id, DatePayment = date, Sum = sum};

            var mockLoanRepository = new Mock<IRepository<Loan>>();
            var mockExpenseRepository = new Mock<IRepository<Expense>>();
            mockLoanRepository.Setup(rep => rep.Add(It.IsAny<Loan>())).Returns(expected);
            mockExpenseRepository.Setup(rep => rep.Add(It.IsAny<LoanPayment>())).Returns(payment);
            loanService = new LoanService(mockLoanRepository.Object, mockExpenseRepository.Object);

            Loan actual = loanService.AddLoan(user, loanSum, clearanceDate, amountDie, repaymentPeriod, creditInstitutionName, bankAddress);

            Assert.AreEqual(expected, actual);
        }

        #region проверяем свойства сущностей
        [TestMethod]
        public void CheckLoanUser()
        {
            Loan actual = new Loan
            {
                UserId = user.Id,
                LoanSum = loanSum,
                AmountDie = amountDie,
                BankAddress = bankAddress,
                CreditInstitutionName = creditInstitutionName,
                RepaymentPeriod = repaymentPeriod,
                ClearanceDate = clearanceDate,
                PaymentsSchedule = new Dictionary<DateTime, float>()
            };

            Assert.AreEqual(user, actual.User);
        }

        [TestMethod]
        public void CheckLoanUserId()
        {
            Loan actual = new Loan
            {
                UserId = user.Id,
                LoanSum = loanSum,
                AmountDie = amountDie,
                BankAddress = bankAddress,
                CreditInstitutionName = creditInstitutionName,
                RepaymentPeriod = repaymentPeriod,
                ClearanceDate = clearanceDate,
                PaymentsSchedule = new Dictionary<DateTime, float>()
            };

            Assert.AreEqual(user.Id, actual.UserId);
        }

        [TestMethod]
        public void CheckLoanClearanceDate()
        {
            Loan actual = new Loan
            {
                UserId = user.Id,
                LoanSum = loanSum,
                AmountDie = amountDie,
                BankAddress = bankAddress,
                CreditInstitutionName = creditInstitutionName,
                RepaymentPeriod = repaymentPeriod,
                ClearanceDate = clearanceDate,
                PaymentsSchedule = new Dictionary<DateTime, float>()
            };

            Assert.AreEqual(clearanceDate,actual.ClearanceDate);
        }

        [TestMethod]
        public void CheckLoanSum()
        {
            Loan actual = new Loan
            {
                UserId = user.Id,
                LoanSum = loanSum,
                AmountDie = amountDie,
                BankAddress = bankAddress,
                CreditInstitutionName = creditInstitutionName,
                RepaymentPeriod = repaymentPeriod,
                ClearanceDate = clearanceDate,
                PaymentsSchedule = new Dictionary<DateTime, float>()
            };

            Assert.AreEqual(loanSum, actual.LoanSum);
        }

        [TestMethod]
        public void CheckLoanAmountDie()
        {
            Loan actual = new Loan
            {
                UserId = user.Id,
                LoanSum = loanSum,
                AmountDie = amountDie,
                BankAddress = bankAddress,
                CreditInstitutionName = creditInstitutionName,
                RepaymentPeriod = repaymentPeriod,
                ClearanceDate = clearanceDate,
                PaymentsSchedule = new Dictionary<DateTime, float>()
            };

            Assert.AreEqual(amountDie, actual.AmountDie);
        }

        [TestMethod]
        public void CheckLoanRepaymentPeriod()
        {
            Loan actual = new Loan
            {
                UserId = user.Id,
                LoanSum = loanSum,
                AmountDie = amountDie,
                BankAddress = bankAddress,
                CreditInstitutionName = creditInstitutionName,
                RepaymentPeriod = repaymentPeriod,
                ClearanceDate = clearanceDate,
                PaymentsSchedule = new Dictionary<DateTime, float>()
            };

            Assert.AreEqual(repaymentPeriod,actual.RepaymentPeriod);
        }

        [TestMethod]
        public void CheckLoanInstitutionName()
        {
            Loan actual = new Loan
            {
                UserId = user.Id,
                LoanSum = loanSum,
                AmountDie = amountDie,
                BankAddress = bankAddress,
                CreditInstitutionName = creditInstitutionName,
                RepaymentPeriod = repaymentPeriod,
                ClearanceDate = clearanceDate,
                PaymentsSchedule = new Dictionary<DateTime, float>()
            };

            Assert.AreEqual(creditInstitutionName, actual.CreditInstitutionName);
        }

        [TestMethod]
        public void CheckLoanBankAddress()
        {
            Loan actual = new Loan
            {
                UserId = user.Id,
                LoanSum = loanSum,
                AmountDie = amountDie,
                BankAddress = bankAddress,
                CreditInstitutionName = creditInstitutionName,
                RepaymentPeriod = repaymentPeriod,
                ClearanceDate = clearanceDate,
                PaymentsSchedule = new Dictionary<DateTime, float>()
            };

            Assert.AreEqual(bankAddress, actual.BankAddress);
        }

        [TestMethod]
        public void CheckLoanPaymentSchedule()
        {
            Loan actual = new Loan
            {
                UserId = user.Id,
                LoanSum = loanSum,
                AmountDie = amountDie,
                BankAddress = bankAddress,
                CreditInstitutionName = creditInstitutionName,
                RepaymentPeriod = repaymentPeriod,
                ClearanceDate = clearanceDate,
                PaymentsSchedule = new Dictionary<DateTime, float>()
            };

            Dictionary<DateTime, float> expectedSchedule = new Dictionary<DateTime, float>();

                var date = actual.ClearanceDate.AddMonths(1);
                var sum = actual.AmountDie / actual.RepaymentPeriod;
                expectedSchedule.Add(date, sum);

            Assert.AreEqual(expectedSchedule, actual.PaymentsSchedule);
        }
        #endregion

        [TestMethod]
        public void GetByIdTest()
        {
            Loan loan = new Loan
            {
                UserId = user.Id,
                LoanSum = loanSum,
                AmountDie = amountDie,
                BankAddress = bankAddress,
                CreditInstitutionName = creditInstitutionName,
                RepaymentPeriod = repaymentPeriod,
                ClearanceDate = clearanceDate,
                PaymentsSchedule = new Dictionary<DateTime, float>()
            };

            var mockLoanRepository = new Mock<IRepository<Loan>>();
            var mockExpenseRepository = new Mock<IRepository<Expense>>();
            mockLoanRepository.Setup(rep => rep.All().SingleOrDefault(x => x.Id == 0)).Returns(loan);
            loanService = new LoanService(mockLoanRepository.Object, mockExpenseRepository.Object);

            Loan actual = loanService.GetById(0);

            Assert.AreEqual(loan, actual);
        }

        [TestMethod]
        public void GetAllTest()
        {
            Loan loan1 = new Loan
            {
                UserId = user.Id,
                LoanSum = loanSum,
                AmountDie = amountDie,
                BankAddress = bankAddress,
                CreditInstitutionName = creditInstitutionName,
                RepaymentPeriod = repaymentPeriod,
                ClearanceDate = clearanceDate,
                PaymentsSchedule = new Dictionary<DateTime, float>()
            };
            Loan loan2 = new Loan{ 
                UserId = user.Id,
                LoanSum = 100, 
                ClearanceDate = new DateTime(2020, 04, 10),
                AmountDie = 1, 
                RepaymentPeriod = 12, 
                CreditInstitutionName = "ERF", 
                BankAddress = "Подъём 10",
                PaymentsSchedule = new Dictionary<DateTime, float>()
            };
            Loan loan3 = new Loan {
                UserId = user.Id, 
                LoanSum = 66666, 
                ClearanceDate= new DateTime(2020, 06, 29), 
                AmountDie =  666, 
                RepaymentPeriod= 6, 
                CreditInstitutionName = "ДЕмон Инд", 
                BankAddress = "Ад 666",
                PaymentsSchedule = new Dictionary<DateTime, float>()
            };

            List<Loan> expected = new List<Loan>() { loan1,loan2, loan3};

            var mockLoanRepository = new Mock<IRepository<Loan>>();
            var mockExpenseRepository = new Mock<IRepository<Expense>>();
            mockLoanRepository.Setup(rep => rep.All().Where(x=> x.UserId == 0)).Returns(expected.AsQueryable<Loan>);
            loanService = new LoanService(mockLoanRepository.Object, mockExpenseRepository.Object);

            IEnumerable<Loan> actual = loanService.GetAll(user);

            CollectionAssert.AreEqual(expected,actual.ToList());
        }

        [TestMethod]
        public void RemoveTest()
        {
            Loan loan1 = new Loan
            {
                UserId = user.Id,
                LoanSum = loanSum,
                AmountDie = amountDie,
                BankAddress = bankAddress,
                CreditInstitutionName = creditInstitutionName,
                RepaymentPeriod = repaymentPeriod,
                ClearanceDate = clearanceDate,
                PaymentsSchedule = new Dictionary<DateTime, float>()
            };
            Loan loan2 = new Loan
            {
                UserId = user.Id,
                LoanSum = 100,
                ClearanceDate = new DateTime(2020, 04, 10),
                AmountDie = 1,
                RepaymentPeriod = 12,
                CreditInstitutionName = "ERF",
                BankAddress = "Подъём 10",
                PaymentsSchedule = new Dictionary<DateTime, float>()
            };
            Loan loan3 = new Loan
            {
                UserId = user.Id,
                LoanSum = 66666,
                ClearanceDate = new DateTime(2020, 06, 29),
                AmountDie = 666,
                RepaymentPeriod = 6,
                CreditInstitutionName = "ДЕмон Инд",
                BankAddress = "Ад 666",
                PaymentsSchedule = new Dictionary<DateTime, float>()
            };

            List<Loan> expected = new List<Loan>() { loan1, loan3 };

            var mockLoanRepository = new Mock<IRepository<Loan>>();
            var mockExpenseRepository = new Mock<IRepository<Expense>>();
            mockLoanRepository.Setup(rep => rep.Remove(loan2));
            mockLoanRepository.Setup(rep => rep.All().Where(x => x.UserId == 0)).Returns(expected.AsQueryable<Loan>);
            loanService = new LoanService(mockLoanRepository.Object, mockExpenseRepository.Object);

            loanService.Remove(loan2);

            IEnumerable<Loan> actual = loanService.GetAll(user);

            CollectionAssert.DoesNotContain(actual.ToList(), loan2);
        }

        [TestMethod]
        public void UpdateTest()
        {
            Loan loan = new Loan
            {
                UserId = user.Id,
                LoanSum = loanSum,
                AmountDie = amountDie,
                BankAddress = bankAddress,
                CreditInstitutionName = creditInstitutionName,
                RepaymentPeriod = repaymentPeriod,
                ClearanceDate = clearanceDate,
                PaymentsSchedule = new Dictionary<DateTime, float>()
            };
            loan.BankAddress = "efxs 5";
            loan.CreditInstitutionName = "hnhsd";

            var mockLoanRepository = new Mock<IRepository<Loan>>();
            var mockExpenseRepository = new Mock<IRepository<Expense>>();
            mockLoanRepository.Setup(rep => rep.Update(loan));
            mockLoanRepository.Setup(rep => rep.All().SingleOrDefault(x => x.Id == 0)).Returns(loan);
            loanService = new LoanService(mockLoanRepository.Object, mockExpenseRepository.Object);

            loanService.UpdateLoan(loan);
            Loan actual = loanService.GetById(0);

            Assert.AreEqual(loan, actual);
        }
    }
}
