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
    public class UserServiceTests
    {
        private LoanContext _dbContext;
        private UserService userService;

        private string _email;
        private string _password;
        private string _firstName;
        private string _lastName;
        private List<Category> _categories;

        [TestInitialize]
        public void TestInitialize()
        {
            _email = "222@mail.ru";
            _password = "12345678";
            _firstName = "Test";
            _lastName = "Testovich";
            _categories = new List<Category>();

            _dbContext = new LoanContext();
            _dbContext.Database.CreateIfNotExists();
            EntityFrameworkRepository<User> userRepository = new EntityFrameworkRepository<User>(_dbContext);
            userService = new UserService(userRepository);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _dbContext.Database.Delete();
        }

        [TestMethod]
        public void AddUserTest()
        {
            User expected = new User
            {
                Id = 1,
                Email = _email,
                Password = _password,
                FirstName = _firstName,
                LastName = _lastName
            
                //Incomes = new List<Income>(),
                //Expenses = new List<Expense>(),
                //Loans = new List<Loan>()
            };
            
            expected.Categories = _categories;

            User actual = userService.Add(_email, _password, _firstName, _lastName);
            _categories.Add(new Category { Id = 1, Name = "Еда", User =actual, UserId = 1 });
            _categories.Add(new Category { Id = 2, Name = "Здоровье", User = actual, UserId = 1 });
            _categories.Add(new Category { Id = 3, Name = "Развлечения", User = actual, UserId = 1 });

            //Assert.AreEqual(expected, actual);
            Assert.AreEqual(_email,actual.Email);
            Assert.AreEqual(_password, actual.Password);
            Assert.AreEqual(_firstName, actual.FirstName);
            Assert.AreEqual(_lastName, actual.LastName);
            Assert.AreEqual(_categories[0].Name ,actual.Categories[0].Name);
            Assert.AreEqual(_categories[1].Name, actual.Categories[1].Name);
            Assert.AreEqual(_categories[2].Name, actual.Categories[2].Name);
            Assert.AreEqual(actual, actual.Categories[1].User);
            Assert.AreEqual(actual, actual.Categories[2].User);
            Assert.AreEqual(actual, actual.Categories[2].User);
        }

        [TestMethod]
        public void GetByIdTest()
        {
            User expected = userService.Add(_email, _password, _firstName, _lastName);

            _categories.Add(new Category { Name = "Еда", User = expected });
            _categories.Add(new Category { Name = "Здоровье", User = expected });
            _categories.Add(new Category { Name = "Развлечения", User = expected });

            User actual = userService.GetById(1);
            Assert.AreEqual(_email, actual.Email);
            Assert.AreEqual(_password, actual.Password);
            Assert.AreEqual(_firstName, actual.FirstName);
            Assert.AreEqual(_lastName, actual.LastName);
            Assert.AreEqual(_categories[0].Name, actual.Categories[0].Name);
            Assert.AreEqual(_categories[1].Name, actual.Categories[1].Name);
            Assert.AreEqual(_categories[2].Name, actual.Categories[2].Name);
            Assert.AreEqual(actual, actual.Categories[1].User);
            Assert.AreEqual(actual, actual.Categories[2].User);
            Assert.AreEqual(actual, actual.Categories[2].User);
        }

        [TestMethod]
        public void ChangeLastNameTest()
        {
            User expected = userService.Add(_email, _password, _firstName, _lastName);

            User actual = userService.GetById(1);

            string newLastName = "Soda";
            userService.ChangeLastName(actual, newLastName);
            //User expected = actual;
            expected.LastName = newLastName;

            actual = userService.GetById(1);
            Assert.AreEqual(newLastName, actual.LastName);
        }

        [TestMethod]
        public void ChangeFirstNameTest()
        {
            User actual = userService.Add(_email, _password, _firstName, _lastName); ;
            string newFirstName = "Avifs";

            userService.ChangeFirstName(actual, newFirstName);
            User expected = actual;
            expected.FirstName = newFirstName;

            actual = userService.GetById(1);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ChangePasswordTest()
        {
            User actual = userService.Add(_email, _password, _firstName, _lastName);
            string newPassword = "sdf4ED294";
            User expected = actual;
            userService.ChangePassword(actual, newPassword);

            expected.Password = newPassword;

            actual = userService.GetById(1);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ChangeEmailTest()
        {
            User actual = userService.Add(_email, _password, _firstName, _lastName);

            string newEmail = "123@mail.com";
            userService.ChangeEmail(actual, newEmail);
            User expected = actual;
            expected.Email = newEmail;

            actual = userService.GetById(1);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetAllTest()
        {
            User user1 = userService.Add(_email, _password, _firstName, _lastName);
            //User user1 = userService.GetById(1);
            User user2 = userService.Add("111@mail.ru", "123456789", "Test2", "Testovichi");
            User user3 = userService.Add("333@mail.ru", "12345678900", "Test3", "Testovichii");

            List<User> expectedUserList = new List<User>() { user1, user2, user3 };

            IEnumerable<User> actualUserList = userService.GetAll();
            CollectionAssert.AreEquivalent(expectedUserList, actualUserList.ToList());
        }

        [TestMethod]
        public void RemoveTest()
        {
            User user1 = userService.Add(_email, _password, _firstName, _lastName);
            User user2 = userService.Add("111@mail.ru", "123456789", "Test2", "Testovichi");
            User user3 = userService.Add("333@mail.ru", "12345678900", "Test3", "Testovichii");
            //User user1 = userService.GetById(1);
            //User user2 = userService.GetById(2);
            //User user3 = userService.GetById(3);

            userService.Remove(user1);
            IEnumerable<User> userData = userService.GetAll();
            CollectionAssert.DoesNotContain(userData.ToList(), user1);
        }
    }
}
