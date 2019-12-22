using System;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Security;
using LoanPortfolio.Db.Entities;
using LoanPortfolio.Services.Interfaces;

namespace LoanPortfolio.Services
{
    public class AuthService : IAuthService
    {
        private const string cookieName = "__AUTH_COOKIE";

        public HttpContext HttpContext { get; set; }

        private IUserService _userService;

        public bool IsLoggedIn()
        {
            throw new System.NotImplementedException();
        }

        public User Login(string username, string password, bool isPersistent)
        {
            var user = _userService.GetAll().SingleOrDefault(x => x.Email == username);

            if (user != null)
            {
                CreateCookie(username, isPersistent);
            }

            return user;
        }

        private void CreateCookie(string userName, bool isPersistent = false)
        {
            var ticket = new FormsAuthenticationTicket(
                1,
                userName,
                DateTime.Now,
                DateTime.Now.Add(FormsAuthentication.Timeout),
                isPersistent,
                string.Empty,
                FormsAuthentication.FormsCookiePath);

            // Encrypt the ticket.
            var encTicket = FormsAuthentication.Encrypt(ticket);

            // Create the cookie.
            var AuthCookie = new HttpCookie(cookieName)
            {
                Value = encTicket,
                Expires = DateTime.Now.Add(FormsAuthentication.Timeout)
            };
            HttpContext.Response.Cookies.Set(AuthCookie);
        }

        public void Logout()
        {
            var httpCookie = HttpContext.Response.Cookies[cookieName];
            if (httpCookie != null)
            {
                httpCookie.Value = string.Empty;
            }
        }
    }
}