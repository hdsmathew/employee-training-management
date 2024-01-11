using Core.Application;
using System;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Unity;

namespace WebMVC
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private ILogger _logger;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            // Configure Unity container
            UnityConfig.RegisterTypes(UnityConfig.Container);
            //FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters); //Disable HandleErrorAttribute
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();

            _logger = UnityConfig.Container.Resolve<ILogger>();
            _logger.LogError(exception, "Unhandled Exception");

            //Server.ClearError();
            //Response.RedirectToRoute("Default", new RouteValueDictionary(new { controller = "Error", action = "Index" }));
        }
    }
}
