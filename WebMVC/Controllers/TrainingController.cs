using Core.Application.Models;
using Core.Application.Repositories;
using Core.Application.Services;
using Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace WebMVC.Controllers
{
    public class TrainingController : SessionController
    {
        private readonly IEnrollmentService _enrollmentService;
        private readonly ITrainingRepository _trainingRepository;
        private readonly ITrainingService _trainingService;

        public TrainingController(ITrainingRepository trainingRepository, ITrainingService trainingService, IEnrollmentService enrollmentService)
        {
            _trainingRepository = trainingRepository;
            _trainingService = trainingService;
            _enrollmentService = enrollmentService;
        }

        public async Task<ActionResult> Index()
        {
            // TODO: Get only valid trainings for user
            IEnumerable<Training> trainings = await _trainingRepository.GetAllWithPrerequisitesAsync();
            return View(trainings);
        }

        [CustomAuthorize(AccountTypeEnum.Admin)]
        public async Task<ActionResult> Create()
        {
            ResponseModel<TrainingViewModel> response = await _trainingService.GetTrainingDetailsAsync();
            if (response.Failure())
            {
                RedirectToAction("Error", "ServerFault");
            }
            return View(response.Entity);
        }

        [CustomAuthorize(AccountTypeEnum.Admin)]
        [HttpPost]
        public async Task<JsonResult> Create(TrainingViewModel model)
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

            Training training = new Training()
            {
                PreferredDepartmentId = model.PreferredDepartmentId,
                RegistrationDeadline = model.RegistrationDeadline,
                SeatsAvailable = model.SeatsAvailable,
                TrainingDescription = model.TrainingDescription,
                TrainingName = model.TrainingName,
            };
            if (model.SelectedPrerequisiteIds != null)
            {
                training.SetPrerequisites(model.SelectedPrerequisiteIds.Select(prerequisiteId => new Prerequisite() { PrerequisiteId = prerequisiteId }));
            }
            ResponseModel<Training> response = await _trainingService.AddAsync(training);

            return Json(
                new
                {
                    Success = response.Success(),
                    Message = response.Success()
                    ? "Training created successfully"
                    : response.GetErrors().FirstOrDefault()?.Message ?? "Training cannot be created"
                },
                "application/json",
                System.Text.Encoding.UTF8);
        }

        [CustomAuthorize(AccountTypeEnum.Admin)]
        public async Task<ActionResult> Edit(short trainingId)
        {
            ResponseModel<TrainingViewModel> response = await _trainingService.GetTrainingDetailsAsync(trainingId);
            if (response.Failure())
            {
                RedirectToAction("Error", "ServerFault");
            }
            response.Entity.TrainingId = trainingId;
            return View(response.Entity);
        }

        [CustomAuthorize(AccountTypeEnum.Admin)]
        [HttpPost]
        public async Task<JsonResult> Edit(TrainingViewModel model)
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

            Training training = new Training()
            {
                TrainingId = model.TrainingId,
                PreferredDepartmentId = model.PreferredDepartmentId,
                RegistrationDeadline = model.RegistrationDeadline,
                SeatsAvailable = model.SeatsAvailable,
                TrainingDescription = model.TrainingDescription,
                TrainingName = model.TrainingName,
            };
            ResponseModel<Training> response = await _trainingService.UpdateAsync(training);

            return Json(
                new
                {
                    Success = response.Success(),
                    Message = response.Success()
                    ? "Training updated successfully"
                    : response.GetErrors().FirstOrDefault()?.Message ?? "Training cannot be updated"
                },
                "application/json",
                System.Text.Encoding.UTF8);
        }

        [CustomAuthorize(AccountTypeEnum.Admin)]
        [HttpPost]
        public async Task<JsonResult> Delete(short trainingId)
        {
            ResponseModel<Training> response = await _trainingService.DeleteAsync(trainingId);
            return Json(
                new
                {
                    Success = response.Success(),
                    Message = response.Success()
                    ? "Training deleted successfully"
                    : response.GetErrors().FirstOrDefault()?.Message ?? "Training cannot be deleted"
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
        public async Task<JsonResult> ValidateApprovedEnrollments(short trainingId)
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
    }
}
