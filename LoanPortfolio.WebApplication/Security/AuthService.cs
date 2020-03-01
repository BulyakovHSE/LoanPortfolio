using System;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using LoanPortfolio.Db.Entities;
using LoanPortfolio.Services.Interfaces;

namespace LoanPortfolio.WebApplication.Security
{
    public class AuthService : IAuthService
    {
        private const string _cookieName = "__AUTH_COOKIE";

        public HttpContext HttpContext { get; set; }

        private IUserService _userService;
        private IPrincipal _currentUser;

        public AuthService(IUserService userService)
        {
            _userService = userService;
        }

        public IPrincipal CurrentUser
        {
            get
            {
                if (_currentUser == null)
                {
                    try
                    {
                        HttpCookie authCookie = HttpContext.Request.Cookies.Get(_cookieName);
                        if (authCookie != null && !string.IsNullOrEmpty(authCookie.Value))
                        {
                            var ticket = FormsAuthentication.Decrypt(authCookie.Value);
                            _currentUser = ticket == null ? new UserProvider(null, null) : new UserProvider(ticket.Name, _userService);
                        }
                        else
                        {
                            _currentUser = new UserProvider(null, null);
                        }
                    }
                    catch (Exception ex)
                    {
                        _currentUser = new UserProvider(null, null);
                    }
                }

                return _currentUser;
            }
        }

        public User Login(string username, string password, bool isPersistent)
        {
            var user = _userService.GetAll().SingleOrDefault(x => x.Email == username);

            if (user != null)
            {
                CreateCookie(username, user.Id, isPersistent);
            }

            return user;
        }

        private void CreateCookie(string userName, int userId, bool isPersistent = false)
        {
            var ticket = new FormsAuthenticationTicket(
                1,
                userName,
                DateTime.Now,
                DateTime.Now.Add(FormsAuthentication.Timeout),
                isPersistent,
                userId.ToString(),
                FormsAuthentication.FormsCookiePath);

            // Encrypt the ticket.
            var encTicket = FormsAuthentication.Encrypt(ticket);

            // Create the cookie.
            var authCookie = new HttpCookie(_cookieName)
            {
                Value = encTicket,
                Expires = DateTime.Now.Add(FormsAuthentication.Timeout)
            };
            HttpContext.Response.Cookies.Set(authCookie);
        }

        public void Logout()
        {
            var httpCookie = HttpContext.Response.Cookies[_cookieName];
            if (httpCookie != null)
            {
                httpCookie.Value = string.Empty;
            }
        }
    }
}