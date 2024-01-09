using Core.Application.Models;
using Core.Application.Repositories;
using Core.Application.Services;
using Core.Domain;
using System.Linq;
using System.Web.Mvc;

namespace WebMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IEmployeeService _employeeService;
        private readonly IEmployeeRepository _employeeRepository;

        public AccountController(IAccountService accountService, IEmployeeService employeeService, IEmployeeRepository employeeRepository)
        {
            _accountService = accountService;
            _employeeService = employeeService;
            _employeeRepository = employeeRepository;
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                ResponseModel<AuthenticatedUser> response = _accountService.Authenticate(model);
                if (response.Success())
                {
                    Session["AuthenticatedUser"] = response.Entity;
                    return RedirectToHomeByRole(response.Entity.AccountType);
                }
                ViewBag.LoginErrorMessage = response.GetErrors()?.FirstOrDefault()?.Message ?? "An error occurred.";
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Login");
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                ResponseModel<Employee> response = _employeeService.Register(model);
                if (response.Success())
                {
                    return RedirectToAction("Login");
                }
                ViewBag.RegistrationErrorMessage = response.GetErrors()?.FirstOrDefault()?.Message ?? "An error occurred.";

            }
            return View(model);
        }

        private ActionResult RedirectToHomeByRole(AccountTypeEnum accountType)
        {
            switch (accountType)
            {
                case AccountTypeEnum.Admin:
                    return RedirectToAction("Index", "Training");

                case AccountTypeEnum.Manager:
                    return RedirectToAction("Index", "Enrollment");

                case AccountTypeEnum.Employee:
                    return RedirectToAction("Index", "Training");

                default:
                    return RedirectToAction("ServerFault", "Error");
            }
        }
    }
}