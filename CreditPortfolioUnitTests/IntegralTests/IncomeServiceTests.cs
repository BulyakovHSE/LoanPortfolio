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
    public class IncomeServiceTests
    {
        private LoanContext _dbContext;
        private IncomeService incomeService;
        private UserService userService;

        User _user;
        string _incomeSource;
        string _incomeSource2;
        DateTime _datePrepaidExpense;
        DateTime _dateSalary;
        DateTime _dateIncome;
        float _prepaidExpanse;
        float _salary;
        float _sum;

        [TestInitialize]
        public void TestInitialize()
        {
            _dbContext = new LoanContext();
            _dbContext.Database.CreateIfNotExists();
            EntityFrameworkRepository<Income> incomeRepository = new EntityFrameworkRepository<Income>(_dbContext);
            EntityFrameworkRepository<User> userRepository = new EntityFrameworkRepository<User>(_dbContext);
            incomeService = new IncomeService(incomeRepository);
            userService = new UserService(userRepository);

            _user = userService.Add("222@mail.ru", "12345", "Test", "Testovich");
            //_user = userService.GetById(1);

            _incomeSource = "Тетсы";
            _incomeSource2 = "ss";
            _datePrepaidExpense = new DateTime(2020, 04, 03);
            _dateSalary = new DateTime(2020, 04, 26);
            _dateIncome = new DateTime(2020, 04, 12);
            _prepaidExpanse = 1000;
            _salary = 4000;
            _sum = 900;

        }

        [TestCleanup]
        public void TestCleanup()
        {
            _dbContext.Database.Delete();
        }

        [TestMethod]
        public void AddRegularIncomeTest()
        {
            RegularIncome expected = new RegularIncome
            {
                UserId = _user.Id,
                IncomeSource = _incomeSource,
                DatePrepaidExpanse = _datePrepaidExpense,
                DateSalary = _dateSalary,
                PrepaidExpanse = _prepaidExpanse,
                Salary = _salary
            };

            RegularIncome actual = incomeService.AddRegularIncome(_user, _incomeSource, _datePrepaidExpense, _prepaidExpanse, _dateSalary, _salary);
            //Assert.AreEqual(expected, actual);
            Assert.AreEqual(expected.IncomeSource, actual.IncomeSource);
            Assert.AreEqual(expected.DatePrepaidExpanse, actual.DatePrepaidExpanse);
            Assert.AreEqual(expected.DateSalary, actual.DateSalary);
            Assert.AreEqual(expected.PrepaidExpanse, actual.PrepaidExpanse);
            Assert.AreEqual(expected.Salary, actual.Salary);
        }

        [TestMethod]
        public void AddPeriodicIncomeTest()
        {
            PeriodicIncome expected = new PeriodicIncome
            {
                UserId = _user.Id,
                IncomeSource = _incomeSource2,
                Sum = _sum,
                DateIncome = _dateIncome
            };

            PeriodicIncome actual = incomeService.AddPeriodicIncome(_user, _incomeSource2, _sum, _dateIncome);
            //Assert.AreEqual(expected, actual);
            Assert.AreEqual(expected.IncomeSource, actual.IncomeSource);
            Assert.AreEqual(expected.Sum, actual.Sum);
            Assert.AreEqual(expected.DateIncome, actual.DateIncome);
        }

        [TestMethod]
        public void GetAllTest()
        {
            RegularIncome regular = incomeService.AddRegularIncome(_user, _incomeSource, _datePrepaidExpense, _prepaidExpanse, _dateSalary, _salary);
            PeriodicIncome periodic = incomeService.AddPeriodicIncome(_user, _incomeSource2, _sum, _dateIncome);

            //RegularIncome regular = (RegularIncome)incomeService.GetById(2);
            //PeriodicIncome periodic = (PeriodicIncome)incomeService.GetById(1);

            List<Income> expected = new List<Income> { regular, periodic };

            IEnumerable<Income> actual = incomeService.GetAll(_user);
            CollectionAssert.AreEqual(expected, actual.ToList());
        }

        [TestMethod]
        public void RemoveRegularIncomeTest()
        {
            RegularIncome regular = incomeService.AddRegularIncome(_user, _incomeSource, _datePrepaidExpense, _prepaidExpanse, _dateSalary, _salary);
            PeriodicIncome periodic = incomeService.AddPeriodicIncome(_user, _incomeSource2, _sum, _dateIncome);

            //RegularIncome regular = (RegularIncome)incomeService.GetById(2);
            //PeriodicIncome periodic = (PeriodicIncome)incomeService.GetById(1);

            List<Income> expected = new List<Income> { periodic };

            incomeService.Remove(regular);

            IEnumerable<Income> incomes = incomeService.GetAll(_user);
            CollectionAssert.DoesNotContain(incomes.ToList(), regular);
        }

        [TestMethod]
        public void RemovePeriodicIncomeTest()
        {
            RegularIncome regular = incomeService.AddRegularIncome(_user, _incomeSource, _datePrepaidExpense, _prepaidExpanse, _dateSalary, _salary);
            PeriodicIncome periodic = incomeService.AddPeriodicIncome(_user, _incomeSource2, _sum, _dateIncome);

            //RegularIncome regular = (RegularIncome)incomeService.GetById(2);
            //PeriodicIncome periodic = (PeriodicIncome)incomeService.GetById(1);

            incomeService.Remove(periodic);

            IEnumerable<Income> incomes = incomeService.GetAll(_user);
            CollectionAssert.DoesNotContain(incomes.ToList(), periodic);
        }

        [TestMethod]
        public void GetByIdRegularTest()
        {
            RegularIncome actual = incomeService.AddRegularIncome(_user, _incomeSource, _datePrepaidExpense, _prepaidExpanse, _dateSalary, _salary);
            //PeriodicIncome periodic = incomeService.AddPeriodicIncome(_user, _incomeSource2, _sum, _dateIncome);

            //RegularIncome actual = (RegularIncome)incomeService.GetById(2);
            //Assert.AreEqual(regular, actual);
            Assert.AreEqual(_incomeSource, actual.IncomeSource);
            Assert.AreEqual(_datePrepaidExpense, actual.DatePrepaidExpanse);
            Assert.AreEqual(_dateSalary, actual.DateSalary);
            Assert.AreEqual(_prepaidExpanse, actual.PrepaidExpanse);
            Assert.AreEqual(_salary, actual.Salary);
        }

        [TestMethod]
        public void GetByIdPeriodicTest()
        {
            //RegularIncome regular = incomeService.AddRegularIncome(_user, _incomeSource, _datePrepaidExpense, _prepaidExpanse, _dateSalary, _salary);
            PeriodicIncome actual = incomeService.AddPeriodicIncome(_user, _incomeSource2, _sum, _dateIncome);

            //PeriodicIncome actual = (PeriodicIncome)incomeService.GetById(1);
            //Assert.AreEqual(periodic, actual);
            Assert.AreEqual(_incomeSource2, actual.IncomeSource);
            Assert.AreEqual(_sum, actual.Sum);
            Assert.AreEqual(_dateIncome, actual.DateIncome);
        }

        [TestMethod]
        public void UpdateRegularIncomeTest()
        {
            RegularIncome regular = incomeService.AddRegularIncome(_user, _incomeSource2, _datePrepaidExpense, _prepaidExpanse, _dateSalary, _salary);
            //RegularIncome regular = (RegularIncome)incomeService.GetById(2);

            regular.Salary = 6599;
            regular.PrepaidExpanse = 3;
            regular.IncomeSource = "SP";
            regular.DatePrepaidExpanse = new DateTime(2020, 04, 06);
            regular.DateSalary = new DateTime(2020, 04, 30);
            incomeService.UpdateIncome(regular);

            RegularIncome actual = (RegularIncome)incomeService.GetById(1);
            Assert.AreEqual(regular, actual);
        }

        [TestMethod]
        public void UpdatePeriodicIncomeTest()
        {
            PeriodicIncome periodic = incomeService.AddPeriodicIncome(_user, _incomeSource2, _sum, _dateIncome);
            //PeriodicIncome periodic = (PeriodicIncome)incomeService.GetById(1);

            periodic.IncomeSource = "SP";
            periodic.Sum = 600;
            periodic.DateIncome = new DateTime(2020, 04, 18);
            incomeService.UpdateIncome(periodic);

            PeriodicIncome actual = (PeriodicIncome)incomeService.GetById(1);
            Assert.AreEqual(periodic, actual);
        }
    }
}
