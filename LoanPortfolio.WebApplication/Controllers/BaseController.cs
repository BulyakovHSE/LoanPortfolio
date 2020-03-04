using LoanPortfolio.Db.Entities;
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

        public User CurrentUser => ((IUserProvider)_authService.CurrentUser.Identity).User;

        public BaseController(IAuthService authService)
        {
            _authService = authService;
        }
    }
}