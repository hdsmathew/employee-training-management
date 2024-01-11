using Core.Application.Models;
using Core.Application.Services;
using Core.Domain;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<JsonResult> GetEmployeeUploads(short employeeId)
        {
            ResponseModel<Employee> response = await _employeeService.GetEmployeeUploadsAsync(employeeId);
            return Json(
                new
                {
                    Success = response.Success(),
                    Message = response.Success()
                    ? "EmployeeUploads retrieved successfully"
                    : response.GetErrors().FirstOrDefault()?.Message ?? "EmployeeUploads cannot be retrieved",
                    Result = response.Success() ? new { EmployeeId = employeeId, response.Entity.EmployeeUploads } : null
                },
                "application/json",
                System.Text.Encoding.UTF8,
                JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetEmployeeUploadsByEnrollmentId(int enrollmentId)
        {
            ResponseModel<EmployeeUpload> response = await _employeeService.GetEmployeeUploadsByEnrollmentIdAsync(enrollmentId);
            return Json(
                new
                {
                    Success = response.Success(),
                    Message = response.Success()
                    ? "EmployeeUploads retrieved successfully"
                    : response.GetErrors().FirstOrDefault()?.Message ?? "EmployeeUploads cannot be retrieved",
                    Result = response.Success() ? new { EmployeeUploads = response.Entities } : null
                },
                "application/json",
                System.Text.Encoding.UTF8,
                JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetManagers()
        {
            ResponseModel<Employee> response = await _employeeService.GetManagersAsync();
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