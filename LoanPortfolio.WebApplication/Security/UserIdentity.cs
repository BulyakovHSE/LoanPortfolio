using System.Linq;
using System.Security.Principal;
using LoanPortfolio.Db.Entities;
using LoanPortfolio.Services.Interfaces;

namespace LoanPortfolio.WebApplication.Security
{
    public class UserIdentity : IIdentity, IUserProvider
    {
        public User User { get; set; }

        public string Name => User == null ? "anonym" : User.FirstName;

        public string AuthenticationType => typeof(User).ToString();

        public bool IsAuthenticated => User != null;

        public void Init(string email, IUserService userService)
        {
            if (!string.IsNullOrWhiteSpace(email))
                User = (User)userService.GetAll().FirstOrDefault(x => x.Email == email)?.Clone();
        }
    }
}