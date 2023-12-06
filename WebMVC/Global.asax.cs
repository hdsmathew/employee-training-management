using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Unity.AspNet.Mvc;
using Unity;

namespace WebMVC
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // Configure Unity container
            IUnityContainer container = new UnityContainer();
            UnityConfig.RegisterTypes(container);

            // Set the dependency resolver to be Unity
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}
