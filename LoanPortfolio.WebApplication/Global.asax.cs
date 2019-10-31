using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using LoanPortfolio.Db.Entities;
using LoanPortfolio.Db.Interfaces;
using LoanPortfolio.Db.Repositories;
using LoanPortfolio.Services;
using LoanPortfolio.Services.Interfaces;
using SimpleInjector;
using SimpleInjector.Integration.Web;
using SimpleInjector.Integration.Web.Mvc;

namespace LoanPortfolio.WebApplication
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Container container = new Container();

            container.Options.DefaultScopedLifestyle = new WebRequestLifestyle();

            container.Register(typeof(IRepository<>), typeof(EntityFrameworkRepository<>));
            container.RegisterInstance(typeof(DbContext), new LoanContext());
            container.Register<IUserService, UserService>();
            container.Register<IIncomeService, IncomeService>();
            container.Register<IExpenseService, ExpenseService>();

            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());

            container.Verify();

            var userService = container.GetInstance<IUserService>();
            if (!userService.GetAll().Any())
            {
                userService.Add("user@mail.ru", "password", "Ivan", "Ivanov");
            }

            var expenseService = container.GetInstance<IExpenseService>();
            var user = userService.GetAll().ToList()[0];
            if (expenseService.GetAll(user).All(x => x.GetType() != typeof(HCSExpense)))
            {
                expenseService.AddHCSExpense(user, DateTime.Now, 9999, "Комментарий");
            }

            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
        }
    }
}
