using Core.Application.Models;
using System.Web.Mvc;
using WebMVC.Custom;

namespace WebMVC.Controllers
{
    [UserSession]
    public class SessionController : Controller
    {
        public AuthenticatedUser AuthenticatedUser
        {
            get
            {
                return Session["AuthenticatedUser"] as AuthenticatedUser;
            }
        }
    }
}