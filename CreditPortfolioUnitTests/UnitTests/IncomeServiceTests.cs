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
    public class IncomeServiceTests
    {
        private IncomeService incomeService;
        //private string DbConnect;

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
            _incomeSource = "Тетсы";
            _incomeSource2 = "ss";
            _datePrepaidExpense = new DateTime(2020, 04, 03);
            _dateSalary = new DateTime(2020, 04, 26);
            _dateIncome = new DateTime(2020,04,12);
            _prepaidExpanse = 1000;
            _salary = 4000;
            _sum = 900;

            //incomeService = new ExpenseService(new IncomeService(EntityFrameworkRepository<Income>(new DbContext(DbConnect))));
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

            var mockRepository = new Mock<IRepository<Income>>();
            mockRepository.Setup(rep => rep.Add(It.IsAny<RegularIncome>())).Returns(expected);
            incomeService = new IncomeService(mockRepository.Object);

            RegularIncome actual = incomeService.AddRegularIncome(_user, _incomeSource, _datePrepaidExpense, _prepaidExpanse, _dateSalary, _salary);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void AddPeriodicIncomeTest()
        {
            PeriodicIncome expected = new PeriodicIncome
            {
                UserId = _user.Id,
                IncomeSource = _incomeSource,
                Sum = _sum,
                DateIncome = _dateIncome
            };

            var mockRepository = new Mock<IRepository<Income>>();
            mockRepository.Setup(rep => rep.Add(It.IsAny<PeriodicIncome>())).Returns(expected);
            incomeService = new IncomeService(mockRepository.Object);

            PeriodicIncome actual = incomeService.AddPeriodicIncome(_user, _incomeSource, _sum, _dateIncome);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetAllTest()
        {
            RegularIncome regular = new RegularIncome
            {
                UserId = _user.Id,
                IncomeSource = _incomeSource,
                DatePrepaidExpanse = _datePrepaidExpense,
                DateSalary = _dateSalary,
                PrepaidExpanse = _prepaidExpanse,
                Salary = _salary
            };
            PeriodicIncome periodic = new PeriodicIncome
            {
                UserId = _user.Id,
                IncomeSource = _incomeSource,
                Sum = _sum,
                DateIncome = _dateIncome
            };

            List<Income> expected = new List<Income> {regular, periodic };

            var mockRepository = new Mock<IRepository<Income>>();
            mockRepository.Setup(rep => rep.All().Where(x => x.UserId == 0)).Returns(expected.AsQueryable<Income>) ;
            incomeService = new IncomeService(mockRepository.Object);

            IEnumerable<Income> actual = incomeService.GetAll(_user);

            CollectionAssert.AreEqual(expected ,actual.ToList());
        }

        [TestMethod]
        public void RemoveRegularIncomeTest()
        {
            RegularIncome regular = new RegularIncome
            {
                UserId = _user.Id,
                IncomeSource = _incomeSource,
                DatePrepaidExpanse = _datePrepaidExpense,
                DateSalary = _dateSalary,
                PrepaidExpanse = _prepaidExpanse,
                Salary = _salary
            };

            PeriodicIncome periodic = new PeriodicIncome
            {
                UserId = _user.Id,
                IncomeSource = _incomeSource,
                Sum = _sum,
                DateIncome = _dateIncome
            };

            List<Income> expected = new List<Income> { periodic };

            var mockRepository = new Mock<IRepository<Income>>();
            mockRepository.Setup(rep => rep.Remove(It.IsAny<Income>()));
            mockRepository.Setup(rep => rep.All().Where(x => x.UserId == 0)).Returns(expected.AsQueryable<Income>);
            incomeService = new IncomeService(mockRepository.Object);

            incomeService.Remove(regular);

            IEnumerable<Income> incomes = incomeService.GetAll(_user);

            CollectionAssert.DoesNotContain(incomes.ToList(),regular);
        }

        //[TestMethod]
        //public void RemovePeriodicIncomeTest()
        //{
        //    User user = new User
        //    {
        //        Id = 1,
        //        Email = "222@mail.ru",
        //        Password = "12345",
        //        FirstName = "Test",
        //        LastName = "Yestovish",
        //        Incomes = new List<Income>(),
        //        Expenses = new List<Expense>(),
        //        Loans = new List<Loan>()
        //    };

        //    RegularIncome regularIncome = incomeService.AddRegularIncome(user, "Тесты", new DateTime(2020, 04, 03), 300, new DateTime(2020, 04, 26), 6000);
        //    PeriodicIncome periodicIncome = incomeService.AddPeriodicIncome(user, "STD", 90, new DateTime(2020, 04, 06));

        //    incomeService.Remove(periodicIncome);

        //    IEnumerable<Income> incomes = incomeService.GetAll(user);

        //    CollectionAssert.DoesNotContain(incomes.ToList(), periodicIncome);
        //}

        [TestMethod]
        public void GetByIdRegularTest()
        {
            RegularIncome regular = new RegularIncome
            {
                UserId = _user.Id,
                IncomeSource = _incomeSource,
                DatePrepaidExpanse = _datePrepaidExpense,
                DateSalary = _dateSalary,
                PrepaidExpanse = _prepaidExpanse,
                Salary = _salary
            };
            PeriodicIncome periodic = new PeriodicIncome
            {
                UserId = _user.Id,
                IncomeSource = _incomeSource,
                Sum = _sum,
                DateIncome = _dateIncome
            };

            var mockRepository = new Mock<IRepository<Income>>();
            mockRepository.Setup(rep => rep.All().SingleOrDefault(x => x.Id == 0)).Returns(regular);
            incomeService = new IncomeService(mockRepository.Object);

            RegularIncome actual = (RegularIncome)incomeService.GetById(0);

            Assert.AreEqual(regular, (RegularIncome)actual);
        }

        //[TestMethod]
        //public void GetByIdPeriodicTest()
        //{
        //    User user = new User
        //    {
        //        Id = 1,
        //        Email = "222@mail.ru",
        //        Password = "12345",
        //        FirstName = "Test",
        //        LastName = "Yestovish",
        //        Incomes = new List<Income>(),
        //        Expenses = new List<Expense>(),
        //        Loans = new List<Loan>()
        //    };

        //    RegularIncome regularIncome = incomeService.AddRegularIncome(user, "Тесты", new DateTime(2020, 04, 03), 300, new DateTime(2020, 04, 26), 6000);
        //    PeriodicIncome periodicIncome = incomeService.AddPeriodicIncome(user, "STD", 90, new DateTime(2020, 04, 06));

        //    PeriodicIncome actual = (PeriodicIncome)incomeService.GetById(1);

        //    Assert.AreEqual(regularIncome, (PeriodicIncome)actual);
        //}

        [TestMethod]
        public void UpdateRegularIncomeTest()
        {

            RegularIncome regular = new RegularIncome
            {
                UserId = _user.Id,
                IncomeSource = _incomeSource,
                DatePrepaidExpanse = _datePrepaidExpense,
                DateSalary = _dateSalary,
                PrepaidExpanse = _prepaidExpanse,
                Salary = _salary
            };
            regular.Salary = 6599;
            regular.PrepaidExpanse = 3;
            regular.IncomeSource = "SP";
            regular.DatePrepaidExpanse = new DateTime(2020, 04, 06);
            regular.DateSalary = new DateTime(2020,04,30);

            var mockRepository = new Mock<IRepository<Income>>();
            mockRepository.Setup(rep => rep.Update(It.IsAny<Income>()));
            mockRepository.Setup(rep => rep.All().SingleOrDefault(x => x.Id == 0)).Returns(regular);
            incomeService = new IncomeService(mockRepository.Object);

            incomeService.UpdateIncome(regular);
            RegularIncome actual = (RegularIncome)incomeService.GetById(0);

            Assert.AreEqual(regular, actual);
        }

        //[TestMethod]
        //public void UpdatePeriodicIncomeTest()
        //{
        //    User user = new User
        //    {
        //        Id = 1,
        //        Email = "222@mail.ru",
        //        Password = "12345",
        //        FirstName = "Test",
        //        LastName = "Yestovish",
        //        Incomes = new List<Income>(),
        //        Expenses = new List<Expense>(),
        //        Loans = new List<Loan>()
        //    };

        //    PeriodicIncome periodicIncome = incomeService.AddPeriodicIncome(user, "STD", 90, new DateTime(2020, 04, 06));
        //    periodicIncome.IncomeSource = "SP";
        //    periodicIncome.Sum = 600;
        //    periodicIncome.DateIncome = new DateTime(2020, 04, 18);

        //    incomeService.UpdateIncome(periodicIncome);
        //    PeriodicIncome actual = (PeriodicIncome)incomeService.GetById(0);

        //    Assert.AreEqual(periodicIncome, actual);
        //}

        //[TestMethod]
        //public void CheckUserTest1()
        //{
        //    RegularIncome actual = new RegularIncome
        //    {
        //        UserId = _user.Id,
        //        IncomeSource = _incomeSource,
        //        DatePrepaidExpanse = _datePrepaidExpense,
        //        DateSalary = _dateSalary,
        //        PrepaidExpanse = _prepaidExpanse,
        //        Salary = _salary
        //    };
        //    Assert.AreEqual(_user,actual.User);
        //}

        //[TestMethod]
        //public void CheckUserId1()
        //{
        //    RegularIncome actual = new RegularIncome
        //    {
        //        UserId = _user.Id,
        //        IncomeSource = _incomeSource,
        //        DatePrepaidExpanse = _datePrepaidExpense,
        //        DateSalary = _dateSalary,
        //        PrepaidExpanse = _prepaidExpanse,
        //        Salary = _salary
        //    };
        //    Assert.AreEqual(_user.Id, actual.UserId);
        //}

        //[TestMethod]
        //public void CheckIncomeSource1()
        //{
        //    RegularIncome actual = new RegularIncome
        //    {
        //        UserId = _user.Id,
        //        IncomeSource = _incomeSource,
        //        DatePrepaidExpanse = _datePrepaidExpense,
        //        DateSalary = _dateSalary,
        //        PrepaidExpanse = _prepaidExpanse,
        //        Salary = _salary
        //    };
        //    Assert.AreEqual(_incomeSource, actual.IncomeSource);
        //}

        //[TestMethod]
        //public void CheckDatePrepaid()
        //{
        //    RegularIncome actual = new RegularIncome
        //    {
        //        UserId = _user.Id,
        //        IncomeSource = _incomeSource,
        //        DatePrepaidExpanse = _datePrepaidExpense,
        //        DateSalary = _dateSalary,
        //        PrepaidExpanse = _prepaidExpanse,
        //        Salary = _salary
        //    };
        //    Assert.AreEqual(_datePrepaidExpense, actual.DatePrepaidExpanse);
        //}

        //[TestMethod]
        //public void CheckDateSallary()
        //{
        //    RegularIncome actual = new RegularIncome
        //    {
        //        UserId = _user.Id,
        //        IncomeSource = _incomeSource,
        //        DatePrepaidExpanse = _datePrepaidExpense,
        //        DateSalary = _dateSalary,
        //        PrepaidExpanse = _prepaidExpanse,
        //        Salary = _salary
        //    };
        //    Assert.AreEqual(_dateSalary, actual.DateSalary);
        //}

        //[TestMethod]
        //public void CheckPrepaid()
        //{
        //    RegularIncome actual = new RegularIncome
        //    {
        //        UserId = _user.Id,
        //        IncomeSource = _incomeSource,
        //        DatePrepaidExpanse = _datePrepaidExpense,
        //        DateSalary = _dateSalary,
        //        PrepaidExpanse = _prepaidExpanse,
        //        Salary = _salary
        //    };
        //    Assert.AreEqual(_prepaidExpanse, actual.PrepaidExpanse);
        //}

        //[TestMethod]
        //public void CheckSalary()
        //{
        //    RegularIncome actual = new RegularIncome
        //    {
        //        UserId = _user.Id,
        //        IncomeSource = _incomeSource,
        //        DatePrepaidExpanse = _datePrepaidExpense,
        //        DateSalary = _dateSalary,
        //        PrepaidExpanse = _prepaidExpanse,
        //        Salary = _salary
        //    };
        //    Assert.AreEqual(_salary, actual.Salary);
        //}

        //[TestMethod]
        //public void CheckUserTest2()
        //{
        //    PeriodicIncome actual = new PeriodicIncome
        //    {
        //        UserId = _user.Id,
        //        IncomeSource = _incomeSource,
        //        Sum = _sum,
        //        DateIncome = _dateIncome
        //    };
        //    Assert.AreEqual(_user, actual.User);
        //}

        //[TestMethod]
        //public void CheckUserId2()
        //{
        //    PeriodicIncome actual = new PeriodicIncome
        //    {
        //        UserId = _user.Id,
        //        IncomeSource = _incomeSource,
        //        Sum = _sum,
        //        DateIncome = _dateIncome
        //    };
        //    Assert.AreEqual(_user.Id, actual.UserId);
        //}

        //[TestMethod]
        //public void CheckIncomeSource2()
        //{
        //    PeriodicIncome actual = new PeriodicIncome
        //    {
        //        UserId = _user.Id,
        //        IncomeSource = _incomeSource2,
        //        Sum = _sum,
        //        DateIncome = _dateIncome
        //    };
        //    Assert.AreEqual(_incomeSource2, actual.IncomeSource);
        //}

        //[TestMethod]
        //public void CheckSum()
        //{
        //    PeriodicIncome actual = new PeriodicIncome
        //    {
        //        UserId = _user.Id,
        //        IncomeSource = _incomeSource,
        //        Sum = _sum,
        //        DateIncome = _dateIncome
        //    };
        //    Assert.AreEqual(_sum, actual.Sum);
        //}

        //[TestMethod]
        //public void CheckDateIncome()
        //{
        //    PeriodicIncome actual = new PeriodicIncome
        //    {
        //        UserId = _user.Id,
        //        IncomeSource = _incomeSource,
        //        Sum = _sum,
        //        DateIncome = _dateIncome
        //    };
        //    Assert.AreEqual(_dateIncome, actual.DateIncome);
        //}
    }
}
