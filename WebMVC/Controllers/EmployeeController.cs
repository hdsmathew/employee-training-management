using Core.Application.Models;
using Core.Application.Services;
using Core.Domain;
using System.Linq;
using System.Web.Mvc;

namespace WebMVC.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        public JsonResult GetEmployeeUploads(short employeeId)
        {
            ResponseModel<Employee> response = _employeeService.GetEmployeeUploads(employeeId);
            return Json(
                new
                {
                    Success = response.Success(),
                    Message = response.Success()
                    ? "EmployeeUpload retrieved successfully"
                    : response.GetErrors().FirstOrDefault()?.Message ?? "EmployeeUploads cannot be retrieved",
                    Result = response.Success() ? new { EmployeeId = employeeId, response.Entity.EmployeeUploads } : null
                },
                "application/json",
                System.Text.Encoding.UTF8,
                JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetEmployeeUploadsByEnrollmentId(short employeeId, int enrollmentId)
        {
            // TODO: Fetch uploads for an enrollment
            ResponseModel<Employee> response = _employeeService.GetEmployeeUploads(employeeId);
            return Json(
                new
                {
                    Success = response.Success(),
                    Message = response.Success()
                    ? "EmployeeUpload retrieved successfully"
                    : response.GetErrors().FirstOrDefault()?.Message ?? "EmployeeUploads cannot be retrieved",
                    Result = response.Success() ? new { EmployeeId = employeeId, response.Entity.EmployeeUploads } : null
                },
                "application/json",
                System.Text.Encoding.UTF8,
                JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetManagers()
        {
            ResponseModel<Employee> response = _employeeService.GetManagers();
            return Json(
                new
                {
                    Success = response.Success(),
                    Message = response.Success()
                    ? "Managers retrieved successfully"
                    : response.GetErrors().FirstOrDefault()?.Message ?? "Managers cannot be retrieved",
                    Result = response.Success() ? new { Managers = response.Entities } : null
                },
                "application/json",
                System.Text.Encoding.UTF8,
                JsonRequestBehavior.AllowGet);
        }
    }
}