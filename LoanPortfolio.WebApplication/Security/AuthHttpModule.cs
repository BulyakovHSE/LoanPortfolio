using System;
using System.Web;
using System.Web.Mvc;
using LoanPortfolio.Services.Interfaces;

namespace LoanPortfolio.WebApplication.Security
{
    public class AuthHttpModule : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.AuthenticateRequest += Authenticate;
        }

        private void Authenticate(object source, EventArgs e)
        {
            HttpApplication app = (HttpApplication)source;
            HttpContext context = app.Context;

            var auth = DependencyResolver.Current.GetService<IAuthService>();
            auth._userService = DependencyResolver.Current.GetService<IUserService>();
            auth.HttpContext = context;
            context.User = auth.CurrentUser;
        }

        public void Dispose()
        {
        }
    }
}