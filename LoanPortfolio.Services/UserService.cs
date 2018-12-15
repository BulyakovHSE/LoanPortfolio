using System.Collections.Generic;
using LoanPortfolio.Db.Entities;
using LoanPortfolio.Services.Interfaces;

namespace LoanPortfolio.Services
{
    public class UserService : IUserService
    {
        public User Add(string email, string pass, string firstName, string lastName)
        {
            throw new System.NotImplementedException();
        }

        public User GetById(int id)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<User> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public void ChangeLastName(User user, string lastName)
        {
            throw new System.NotImplementedException();
        }

        public void ChangeFirstName(User user, string firstName)
        {
            throw new System.NotImplementedException();
        }

        public void ChangePassword(User user, string password)
        {
            throw new System.NotImplementedException();
        }

        public void ChangeEmail(User user, string email)
        {
            throw new System.NotImplementedException();
        }

        public void Remove(User user)
        {
            throw new System.NotImplementedException();
        }
    }
}