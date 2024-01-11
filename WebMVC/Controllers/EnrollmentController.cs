﻿using Core.Application.Models;
using Core.Application.Services;
using Core.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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
        public async Task<ActionResult> Index()
        {
            ResponseModel<EnrollmentViewModel> response = await GetEnrollmentViewModelsByRoleAsync(AuthenticatedUser.AccountType);
            if (response is null || response.Failure())
            {
                RedirectToAction("Error", "ServerFault");
            }
            return View(response.Entities);
        }

        [CustomAuthorize(AccountTypeEnum.Manager)]
        [HttpPost]
        public async Task<JsonResult> Approve(int enrollmentId)
        {
            ResponseModel<EnrollmentViewModel> response = await _enrollmentService.ApproveAsync(enrollmentId, AuthenticatedUser.AccountId);
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
        public async Task<JsonResult> Decline(DeclineEnrollmentViewModel declineEnrollmentViewModel)
        {
            // TODO: Validate decline reason message
            ResponseModel<EnrollmentViewModel> response = await _enrollmentService.DeclineAsync(declineEnrollmentViewModel, AuthenticatedUser.AccountId);
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

        [CustomAuthorize(AccountTypeEnum.Admin)]
        [HttpPost]
        public async Task<JsonResult> GenerateEnrollmentsReport(short trainingId)
        {
            // TODO: Implmenent Excel report
            throw new NotImplementedException();
        }

        [CustomAuthorize(AccountTypeEnum.Admin)]
        [HttpPost]
        public async Task<JsonResult> GenerateEnrollmentsReportByTraining(short trainingId)
        {
            // TODO: Implmenent Excel report
            throw new NotImplementedException();
        }

        [CustomAuthorize(AccountTypeEnum.Manager, AccountTypeEnum.Employee)]
        public FileContentResult GetDocumentUpload(string uploadedFileName)
        {
            // TODO: Add validation
            string uploadsFolder = Server.MapPath("~/App_Data/Employee_Uploads");
            string filePath = Path.Combine(uploadsFolder, uploadedFileName);
            byte[] fileContents = System.IO.File.ReadAllBytes(filePath);
            string mimeType = MimeMapping.GetMimeMapping(uploadedFileName);
            return File(fileContents, mimeType, uploadedFileName);
        }

        [CustomAuthorize(AccountTypeEnum.Manager, AccountTypeEnum.Employee)]
        [HttpPost]
        public async Task<JsonResult> SubmitEnrollment(EnrollmentSubmissionViewModel enrollmentSubmissionViewModel)
        {
            // Refactor: Redo logic to remove additional method for submitting enrollments without uploads
            // Refactor: Move upload functionality to service
            // TODO : Validate file input
            ResponseModel<Enrollment> response;
            if (enrollmentSubmissionViewModel.EmployeeUploads is null)
            {
                response = await _enrollmentService.SubmitAsync(AuthenticatedUser.EmployeeId, enrollmentSubmissionViewModel.TrainingId);
            }
            else
            {
                IEnumerable<EmployeeUpload> employeeUploads = SaveUploadedFiles(enrollmentSubmissionViewModel.EmployeeUploads, enrollmentSubmissionViewModel.PrerequisiteIds);
                response = await _enrollmentService.SubmitAsync(AuthenticatedUser.EmployeeId, enrollmentSubmissionViewModel.TrainingId, employeeUploads);
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

        [CustomAuthorize(AccountTypeEnum.Admin)]
        [HttpPost]
        public async Task<JsonResult> ValidateApprovedEnrollments()
        {
            ResponseModel<ResponseModel<Enrollment>> response = await _enrollmentService.ValidateApprovedEnrollmentsAsync(AuthenticatedUser.AccountId);
            return Json(
                new
                {
                    Success = response.Success(),
                    Message = response.Success()
                    ? "All enrollments processed successfully"
                    : response.GetErrors().FirstOrDefault()?.Message ?? "Enrollments cannot be processed"
                },
                "application/json",
                System.Text.Encoding.UTF8);
        }

        [CustomAuthorize(AccountTypeEnum.Admin)]
        [HttpPost]
        public async Task<JsonResult> ValidateApprovedEnrollmentsByTraining(short trainingId)
        {
            ResponseModel<Enrollment> response = await _enrollmentService.ValidateApprovedEnrollmentsByTrainingAsync(AuthenticatedUser.AccountId, trainingId);
            return Json(
                new
                {
                    Success = response.Success(),
                    Message = response.Success()
                    ? "Enrollments for training processed successfully"
                    : response.GetErrors().FirstOrDefault()?.Message ?? "Enrollment for trainings cannot be processed"
                },
                "application/json",
                System.Text.Encoding.UTF8);
        }

        private async Task<ResponseModel<EnrollmentViewModel>> GetEnrollmentViewModelsByRoleAsync(AccountTypeEnum accountType)
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