using Core.Application.Repositories;
using Core.Domain.User;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace WebMVC.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public JsonResult Index()
        {
            List<User> users = _userRepository.GetAll().ToList();

            return Json(users, "application/json", System.Text.Encoding.UTF8, JsonRequestBehavior.AllowGet);
        }
    }
}