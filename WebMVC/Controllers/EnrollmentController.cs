using Core.Application.Models;
using Core.Application.Services;
using Core.Domain;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMVC.ViewModels;

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
        public ActionResult Index()
        {
            ResponseModel<EnrollmentViewModel> response = GetEnrollmentViewModelsByRole(AuthenticatedUser.AccountType);
            if (response is null || response.Failure())
            {
                RedirectToAction("Error", "ServerFault");
            }
            return View(response.Entities);
        }

        [CustomAuthorize(AccountTypeEnum.Manager)]
        [HttpPost]
        public JsonResult Approve(int enrollmentId)
        {
            ResponseModel<EnrollmentViewModel> response = _enrollmentService.Approve(enrollmentId, AuthenticatedUser.AccountId);
            return Json(
                new
                {
                    Success = response.Success(),
                    Message = response.Success()
                    ? "Enrollment approved successfully"
                    : response.GetErrors().FirstOrDefault()?.Message ?? "Enrollment application cannot be approved"
                },
                "application/json",
                System.Text.Encoding.UTF8);
        }

        [CustomAuthorize(AccountTypeEnum.Manager)]
        [HttpPost]
        public JsonResult Decline(DeclineEnrollmentViewModel declineEnrollmentViewModel)
        {
            // TODO: Validate decline reason message
            ResponseModel<EnrollmentViewModel> response = _enrollmentService.Decline(declineEnrollmentViewModel, AuthenticatedUser.AccountId);
            return Json(
                new
                {
                    Success = response.Success(),
                    Message = response.Success()
                    ? "Enrollment declined successfully"
                    : response.GetErrors().FirstOrDefault()?.Message ?? "Enrollment application cannot be declined"
                },
                "application/json",
                System.Text.Encoding.UTF8);
        }

        [CustomAuthorize(AccountTypeEnum.Manager, AccountTypeEnum.Employee)]
        [HttpPost]
        public JsonResult SubmitEnrollment(EnrollmentSubmissionViewModel enrollmentSubmissionViewModel)
        {
            // TODO : Validate file input
            ResponseModel<Enrollment> response;
            if (enrollmentSubmissionViewModel.EmployeeUploads is null)
            {
                response = _enrollmentService.Submit(AuthenticatedUser.EmployeeId, enrollmentSubmissionViewModel.TrainingId);
            }
            else
            {
                IEnumerable<EmployeeUpload> employeeUploads = SaveUploadedFiles(enrollmentSubmissionViewModel.EmployeeUploads, enrollmentSubmissionViewModel.PrerequisiteIds);
                response = _enrollmentService.Submit(AuthenticatedUser.EmployeeId, enrollmentSubmissionViewModel.TrainingId, employeeUploads);
            }

            return Json(
                new
                {
                    Success = response.Success(),
                    Message = response.Success()
                    ? "Enrollment submitted successfully"
                    : response.GetErrors().FirstOrDefault()?.Message ?? "Enrollment application cannot be submitted"
                },
                "application/json",
                System.Text.Encoding.UTF8);
        }

        private ResponseModel<EnrollmentViewModel> GetEnrollmentViewModelsByRole(AccountTypeEnum accountType)
        {
            switch (accountType)
            {
                case AccountTypeEnum.Manager:
                    return _enrollmentService.GetEnrollmentSubmissionsForApproval(AuthenticatedUser.EmployeeId);

                case AccountTypeEnum.Employee:
                    return _enrollmentService.GetEnrollments(AuthenticatedUser.EmployeeId);

                default:
                    return null;
            }
        }

        private IEnumerable<EmployeeUpload> SaveUploadedFiles(IEnumerable<HttpPostedFileBase> uploadedFiles, IEnumerable<byte> prerequisiteIds)
        {
            string uploadsFolder = Server.MapPath("~/App_Data/Employee_Uploads");

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            return uploadedFiles
                .Zip(prerequisiteIds, (file, prerequisiteId) => new { File = file, PrerequisiteId = prerequisiteId })
                .Select(data => SaveUploadedFile(data.File, data.PrerequisiteId, uploadsFolder))
                .ToList();
        }

        private EmployeeUpload SaveUploadedFile(HttpPostedFileBase uploadedFile, byte prerequisiteId, string uploadsFolder)
        {
            // TODO: Generate unique filename
            string fileName = Path.GetFileName(uploadedFile.FileName);
            string filePath = Path.Combine(uploadsFolder, fileName);
            uploadedFile.SaveAs(filePath);

            return new EmployeeUpload() { PrerequisiteId = prerequisiteId, UploadedFileName = fileName };
        }
    }
}