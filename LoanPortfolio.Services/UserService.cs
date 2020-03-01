using System;
using System.Collections.Generic;
using System.Linq;
using LoanPortfolio.Db.Entities;
using LoanPortfolio.Db.Interfaces;
using LoanPortfolio.Services.Interfaces;

namespace LoanPortfolio.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _userRepository;

        public UserService(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public User Add(string email, string pass, string firstName, string lastName)
        {
            var user = new User {Email = email, Password = pass, FirstName = firstName, LastName = lastName};
            var categories = new List<Category>();
            var cats = new[] {"Еда", "Здоровье", "Развлечения"};
            foreach (var cat in cats)
                categories.Add(new Category{Name = cat, User = user});
            user.Categories = categories;
            _userRepository.Add(user);
            return _userRepository.All().Single(u => u.Email == email);
        }

        public User GetById(int id)
        {
            return _userRepository.All().First(u => u.Id == id);
        }

        public IEnumerable<User> GetAll()
        {
            return _userRepository.All();
        }

        public void ChangeLastName(User user, string lastName)
        {
            user.LastName = lastName;
            _userRepository.Update(user);
        }

        public void ChangeFirstName(User user, string firstName)
        {
            user.FirstName = firstName;
            _userRepository.Update(user);
        }

        public void ChangePassword(User user, string password)
        {
            user.Password = password;
            _userRepository.Update(user);
        }

        public void ChangeEmail(User user, string email)
        {
            if (_userRepository.All().Any(x => x.Email == email))
                throw new ArgumentException("User with this email already exists!");

            user.Email = email;
            _userRepository.Update(user);
        }

        public void Remove(User user)
        {
            _userRepository.Remove(user);
        }
    }
}