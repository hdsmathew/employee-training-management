using System.Web.Mvc;

namespace WebMVC.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult Forbidden()
        {
            return View();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult NotFound()
        {
            return View();
        }

        public ActionResult ServerFault()
        {
            return View();
        }
    }
}