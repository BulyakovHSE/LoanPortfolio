using LoanPortfolio.Db.Entities;
using LoanPortfolio.Services.Interfaces;
using LoanPortfolio.WebApplication.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LoanPortfolio.WebApplication.Controllers
{
    public class BaseController : Controller
    {
        protected IAuthService _authService;
        private readonly IUserService _userService;

        public User CurrentUser;

        public BaseController(IAuthService authService, IUserService userService)
        {
            _authService = authService;
            _userService = userService;

            User user = ((IUserProvider)_authService.CurrentUser.Identity).User;
            if (user != null)
            {
                CurrentUser = _userService.GetById(user.Id);
            }
        }

        public void UpdateNotificationsList()
        {
            var mvc = HttpContext.ApplicationInstance as MvcApplication;
            mvc?.UpdateNotifications(CurrentUser.Id);
        }
    }
}