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
    public class ExpenseServiceTest
    {
        private LoanContext _dbContext;
        private ExpenseService expenseService;
        private UserService userService;

        private User _user;
        private DateTime _datePayment;
        private DateTime _datePayment2;
        private float _sum;
        private float _sum2;
        private Category _category;
        private string _comment;

        [TestInitialize]
        public void TestInitialize()
        {
            _dbContext = new LoanContext();
            _dbContext.Database.CreateIfNotExists();
            EntityFrameworkRepository<Expense> expenseRepository = new EntityFrameworkRepository<Expense>(_dbContext);
            EntityFrameworkRepository<User> userRepository = new EntityFrameworkRepository<User>(_dbContext);
            expenseService = new ExpenseService(expenseRepository);
            userService = new UserService(userRepository);

            _user = userService.Add("222@mail.ru", "12345", "Test", "Testovich");
            //_user = userService.GetById(1);

            _datePayment = new DateTime(2020, 04, 03);
            _datePayment2 = new DateTime(2020, 04, 10);
            _sum = 600;
            _sum2 = 500;
            _category = new Category() { Name = "Развлечение", User = _user };
            _comment = "324";

        }

        [TestCleanup]
        public void TestCleanup()
        {
            _dbContext.Database.Delete();
        }

        [TestMethod]
        public void AddPersonalExpenseTest()
        {
            PersonalExpense expected = new PersonalExpense
            {
                UserId = _user.Id,
                DatePayment = _datePayment,
                Sum = _sum,
                ExpenseCategory = _category
            };

            PersonalExpense actual = expenseService.AddPersonalExpense(_user, _datePayment, _sum, _category);
            //Assert.AreEqual(expected, actual);
            Assert.AreEqual(expected.DatePayment, actual.DatePayment);
            Assert.AreEqual(expected.Sum, actual.Sum);
            Assert.AreEqual(expected.ExpenseCategory, actual.ExpenseCategory);
        }

        [TestMethod]
        public void AddHCExpenseTest()
        {
            HCSExpense expected = new HCSExpense
            {
                UserId = _user.Id,
                DatePayment = _datePayment2,
                Sum = _sum2,
                Comment = _comment
            };

            HCSExpense actual = expenseService.AddHCSExpense(_user, _datePayment2, _sum2, _comment);
            //Assert.AreEqual(expected, actual);
            Assert.AreEqual(expected.DatePayment, actual.DatePayment);
            Assert.AreEqual(expected.Sum, actual.Sum);
            Assert.AreEqual(expected.Comment, actual.Comment);
        }

        [TestMethod]
        public void GetByIdTest()
        {
            PersonalExpense personalExpense = expenseService.AddPersonalExpense(_user, _datePayment, _sum, _category);
            HCSExpense hcsExpense = expenseService.AddHCSExpense(_user, _datePayment2, _sum2, _comment);

            PersonalExpense actual = (PersonalExpense)expenseService.GetById(1);
            Assert.AreEqual(_datePayment, actual.DatePayment);
            Assert.AreEqual(_sum, actual.Sum);
            Assert.AreEqual(_category, actual.ExpenseCategory);
        }

        [TestMethod]
        public void UpdatePersonalExpenseTest()
        {
            PersonalExpense personalExpense = expenseService.AddPersonalExpense(_user, _datePayment, _sum, _category);
            //HCSExpense hcsExpense = expenseService.AddHCSExpense(_user, _datePayment2, _sum2, _comment);

            personalExpense.DatePayment = new DateTime(2020, 04, 01);
            personalExpense.Sum = 9000;
            expenseService.UpdateExpense(personalExpense);

            var actual = (PersonalExpense)expenseService.GetById(1);
            Assert.AreEqual(personalExpense, actual);
        }

        [TestMethod]
        public void UpdateHCSexpenseTest()
        {
            //PersonalExpense personalExpense = expenseService.AddPersonalExpense(_user, _datePayment, _sum, _category);
            HCSExpense hcsExpense = expenseService.AddHCSExpense(_user, _datePayment2, _sum2, _comment);

            hcsExpense.DatePayment = new DateTime(2020, 04, 13);
            hcsExpense.Sum = 1;
            hcsExpense.Comment = "134";
            expenseService.UpdateExpense(hcsExpense);

            HCSExpense actual = (HCSExpense)expenseService.GetById(1);
            Assert.AreEqual(hcsExpense, actual);
        }

        [TestMethod]
        public void GetAllTest()
        {
            PersonalExpense personalExpense = expenseService.AddPersonalExpense(_user, _datePayment, _sum, _category);
            HCSExpense hcsExpense = expenseService.AddHCSExpense(_user, _datePayment2, _sum2, _comment);

            List<Expense> expected = new List<Expense> { personalExpense, hcsExpense };

            IEnumerable<Expense> actual = expenseService.GetAll(_user);
            CollectionAssert.AreEquivalent(expected, actual.ToList());
        }

        [TestMethod]
        public void RemovePersonalExpenseTest()
        {
            PersonalExpense personalExpense = expenseService.AddPersonalExpense(_user, _datePayment, _sum, _category);
            HCSExpense hcsExpense = expenseService.AddHCSExpense(_user, _datePayment2, _sum2, _comment);

            expenseService.Remove(personalExpense);

            IEnumerable<Expense> expenseList = expenseService.GetAll(_user);
            CollectionAssert.DoesNotContain(expenseList.ToList(), personalExpense);
        }

        [TestMethod]
        public void RemoveHCSExpenseTest()
        {
            PersonalExpense personalExpense = expenseService.AddPersonalExpense(_user, _datePayment, _sum, _category);
            HCSExpense hcsExpense = expenseService.AddHCSExpense(_user, _datePayment2, _sum2, _comment);

            expenseService.Remove(hcsExpense);

            IEnumerable<Expense> expenseList = expenseService.GetAll(_user);
            CollectionAssert.DoesNotContain(expenseList.ToList(), hcsExpense);
        }
    }
}
