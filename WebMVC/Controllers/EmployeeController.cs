using Core.Application.Models;
using Core.Application.Services;
using Core.Domain;
using System.Collections.Generic;
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
            ResultT<Employee> result = await _employeeService.GetEmployeeWithUploadsAsync(employeeId);
            return Json(
                new
                {
                    Success = result.IsSuccess,
                    Message = result.IsSuccess
                    ? "EmployeeUploads retrieved successfully"
                    : result.Error.Message ?? "EmployeeUploads cannot be retrieved",
                    Result = result.IsSuccess ? new { EmployeeId = employeeId, result.Value.EmployeeUploads } : null
                },
                "application/json",
                System.Text.Encoding.UTF8,
                JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetEmployeeUploadsByEnrollmentId(int enrollmentId)
        {
            ResultT<IEnumerable<EmployeeUpload>> result = await _employeeService.GetEmployeeUploadsByEnrollmentIdAsync(enrollmentId);
            return Json(
                new
                {
                    Success = result.IsSuccess,
                    Message = result.IsSuccess
                    ? "EmployeeUploads retrieved successfully"
                    : result.Error.Message ?? "EmployeeUploads cannot be retrieved",
                    Result = result.IsSuccess ? new { EmployeeUploads = result.Value } : null
                },
                "application/json",
                System.Text.Encoding.UTF8,
                JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetManagers()
        {
            ResultT<IEnumerable<Employee>> result = await _employeeService.GetManagersAsync();
            return Json(
                new
                {
                    Success = result.IsSuccess,
                    Message = result.IsSuccess
                    ? "Managers retrieved successfully"
                    : result.Error.Message ?? "Managers cannot be retrieved",
                    Result = result.IsSuccess ? new { Managers = result.Value } : null
                },
                "application/json",
                System.Text.Encoding.UTF8,
                JsonRequestBehavior.AllowGet);
        }
    }
}