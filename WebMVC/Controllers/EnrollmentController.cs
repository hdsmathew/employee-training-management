using Core.Application.Models;
using Core.Application.Services;
using Core.Domain;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace WebMVC.Controllers
{
    public class EnrollmentController : SessionController
    {
        private readonly IEnrollmentService _enrollmentService;

        public EnrollmentController(IEnrollmentService enrollmentService)
        {
            _enrollmentService = enrollmentService;
        }

        [CustomAuthorize(AccountTypeEnum.Manager, AccountTypeEnum.Employee)]
        public async Task<ActionResult> Index()
        {
            ResultT<IEnumerable<EnrollmentViewModel>> result = await GetEnrollmentViewModelsByRoleAsync(AuthenticatedUser.AccountType);
            if (result.IsFailure)
            {
                RedirectToAction("Error", "ServerFault");
            }
            return View(result.Value);
        }

        [CustomAuthorize(AccountTypeEnum.Manager)]
        [HttpPost]
        public async Task<JsonResult> Approve(int enrollmentId)
        {
            Result result = await _enrollmentService.ApproveAsync(enrollmentId, AuthenticatedUser.AccountId);
            return Json(
                new
                {
                    Success = result.IsSuccess,
                    Message = result.IsSuccess
                    ? "Enrollment approved successfully"
                    : result.Error.Message ?? "Enrollment application cannot be approved"
                },
                "application/json",
                System.Text.Encoding.UTF8);
        }

        [CustomAuthorize(AccountTypeEnum.Manager)]
        [HttpPost]
        public async Task<JsonResult> Decline(DeclineEnrollmentViewModel declineEnrollmentViewModel)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToList()
                    );
                return Json(
                    new
                    {
                        Success = false,
                        Message = "Validation failed",
                        Errors = errors
                    },
                    "application/json",
                    System.Text.Encoding.UTF8);
            }

            Result result = await _enrollmentService.DeclineAsync(declineEnrollmentViewModel, AuthenticatedUser.AccountId);
            return Json(
                new
                {
                    Success = result.IsSuccess,
                    Message = result.IsSuccess
                    ? "Enrollment declined successfully"
                    : result.Error.Message ?? "Enrollment application cannot be declined"
                },
                "application/json",
                System.Text.Encoding.UTF8);
        }

        [CustomAuthorize(AccountTypeEnum.Admin)]
        public async Task<FileStreamResult> GenerateEnrollmentReport(string fileName)
        {
            ResultT<Stream> result = await _enrollmentService.GenerateEnrollmentReportAsync();
            return File(result.Value, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        [CustomAuthorize(AccountTypeEnum.Admin)]
        public async Task<FileStreamResult> GenerateEnrollmentReportByTraining(short trainingId, string fileName)
        {
            ResultT<Stream> result = await _enrollmentService.GenerateEnrollmentReportByTrainingAsync(trainingId);
            return File(result.Value, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        [CustomAuthorize(AccountTypeEnum.Manager, AccountTypeEnum.Employee)]
        public FileContentResult GetDocumentUpload(string uploadedFileName)
        {
            // TODO: Add validation
            string uploadsFolder = ConfigurationManager.AppSettings["UploadFolderPath"];
            string filePath = Path.Combine(uploadsFolder, uploadedFileName);
            byte[] fileContents = System.IO.File.ReadAllBytes(filePath);
            string mimeType = MimeMapping.GetMimeMapping(uploadedFileName);
            return File(fileContents, mimeType, uploadedFileName);
        }

        [CustomAuthorize(AccountTypeEnum.Manager, AccountTypeEnum.Employee)]
        [HttpPost]
        public async Task<JsonResult> SubmitEnrollment(EnrollmentSubmissionViewModel enrollmentSubmissionViewModel)
        {
            // TODO : Validate file input
            Result result = await _enrollmentService.SubmitAsync(AuthenticatedUser.EmployeeId, enrollmentSubmissionViewModel);
            return Json(
                new
                {
                    Success = result.IsSuccess,
                    Message = result.IsSuccess
                    ? "Enrollment submitted successfully"
                    : result.Error.Message ?? "Enrollment application cannot be submitted"
                },
                "application/json",
                System.Text.Encoding.UTF8);
        }

        [CustomAuthorize(AccountTypeEnum.Admin)]
        [HttpPost]
        public async Task<JsonResult> ValidateApprovedEnrollments()
        {
            ResultT<IEnumerable<(string, Result)>> result = await _enrollmentService.ValidateApprovedEnrollmentsAsync(AuthenticatedUser.AccountId);
            return Json(
                new
                {
                    Success = result.IsSuccess,
                    Message = result.IsSuccess
                    ? "All enrollments processed successfully"
                    : result.Error.Message ?? "Enrollments cannot be processed",
                    Result = result.IsSuccess ? result.Value : null
                },
                "application/json",
                System.Text.Encoding.UTF8);
        }

        [CustomAuthorize(AccountTypeEnum.Admin)]
        [HttpPost]
        public async Task<JsonResult> ValidateApprovedEnrollmentsByTraining(short trainingId)
        {
            Result result = await _enrollmentService.ValidateApprovedEnrollmentsByTrainingAsync(AuthenticatedUser.AccountId, trainingId);
            return Json(
                new
                {
                    Success = result.IsSuccess,
                    Message = result.IsSuccess
                    ? "Enrollments for training processed successfully"
                    : result.Error.Message ?? "Enrollment for trainings cannot be processed"
                },
                "application/json",
                System.Text.Encoding.UTF8);
        }

        private async Task<ResultT<IEnumerable<EnrollmentViewModel>>> GetEnrollmentViewModelsByRoleAsync(AccountTypeEnum accountType)
        {
            switch (accountType)
            {
                case AccountTypeEnum.Manager:
                    return await _enrollmentService.GetEnrollmentSubmissionsForApprovalAsync(AuthenticatedUser.EmployeeId);

                case AccountTypeEnum.Employee:
                    return await _enrollmentService.GetEnrollmentsAsync(AuthenticatedUser.EmployeeId);

                default:
                    return null;
            }
        }
    }
}