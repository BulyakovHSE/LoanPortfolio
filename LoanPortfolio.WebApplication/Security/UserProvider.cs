using System.Security.Principal;
using LoanPortfolio.Services.Interfaces;

namespace LoanPortfolio.WebApplication.Security
{
    public class UserProvider : IPrincipal
    {
        private UserIdentity _userIdentity { get; set; }

        public IIdentity Identity => _userIdentity;

        public bool IsInRole(string role)
        {
            if (_userIdentity.User == null)
            {
                return false;
            }
            // todo: Add roles check here if needed
            return true; 
        }

        public UserProvider(string name, IUserService userService)
        {
            _userIdentity = new UserIdentity();
            _userIdentity.Init(name, userService);
        }


        public override string ToString()
        {
            return _userIdentity.Name;
        }
    }
}