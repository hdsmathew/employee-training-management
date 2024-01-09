using System.Web.Mvc;

namespace WebMVC.Controllers
{
    public class DojoController : Controller
    {
        // GET: Dojo
        public ActionResult Login()
        {
            return View();
        }
    }
}