using Core.Application;
using Infrastructure.Jobs;
using System;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Unity;

namespace WebMVC
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private JobConfig _jobConfig;
        private ILogger _logger;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            UnityConfig.RegisterTypes(UnityConfig.Container);
            //FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters); //Disable HandleErrorAttribute
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            _jobConfig = UnityConfig.Container.Resolve<JobConfig>();
            _jobConfig.ConfigureAndStartScheduler();
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();

            _logger = UnityConfig.Container.Resolve<ILogger>();
            _logger.LogError(exception, "Unhandled Exception");
        }
    }
}
