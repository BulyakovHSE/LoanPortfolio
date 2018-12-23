using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using LoanPortfolio.Db.Interfaces;
using LoanPortfolio.Db.Repositories;
using LoanPortfolio.Services;
using LoanPortfolio.Services.Interfaces;
using SimpleInjector;

namespace LoanPortfolio.WebApplication
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static Container _container;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            _container = new Container();

            _container.Register(typeof(IRepository<>), typeof(EntityFrameworkRepository<>));
            _container.RegisterInstance(typeof(DbContext), new LoanContext());
            _container.Register<IUserService, UserService>();
            _container.Register<IIncomeService, IncomeService>();
            _container.Register<IExpenseService, ExpenseService>();

            _container.Verify();
        }
    }
}
