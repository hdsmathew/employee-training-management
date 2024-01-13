using Core.Application.Models;
using Core.Application.Repositories;
using Core.Application.Services;
using Core.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace WebMVC.Controllers
{
    public class TrainingController : SessionController
    {
        private readonly ITrainingRepository _trainingRepository;
        private readonly ITrainingService _trainingService;

        public TrainingController(ITrainingRepository trainingRepository, ITrainingService trainingService)
        {
            _trainingRepository = trainingRepository;
            _trainingService = trainingService;
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
            ResultT<TrainingViewModel> result = await _trainingService.GetTrainingDetailsAsync();
            if (result.IsFailure)
            {
                RedirectToAction("Error", "ServerFault");
            }
            return View(result.Value);
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
            Result result = await _trainingService.AddAsync(training);

            return Json(
                new
                {
                    Success = result.IsSuccess,
                    Message = result.IsSuccess
                    ? "Training created successfully"
                    : result.Error.Message ?? "Training cannot be created"
                },
                "application/json",
                System.Text.Encoding.UTF8);
        }

        [CustomAuthorize(AccountTypeEnum.Admin)]
        public async Task<ActionResult> Edit(short trainingId)
        {
            ResultT<TrainingViewModel> result = await _trainingService.GetTrainingDetailsAsync(trainingId);
            if (result.IsFailure)
            {
                RedirectToAction("Error", "ServerFault");
            }
            result.Value.TrainingId = trainingId;
            return View(result.Value);
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
            Result result = await _trainingService.UpdateAsync(training);

            return Json(
                new
                {
                    Success = result.IsSuccess,
                    Message = result.IsSuccess
                    ? "Training updated successfully"
                    : result.Error.Message ?? "Training cannot be updated"
                },
                "application/json",
                System.Text.Encoding.UTF8);
        }

        [CustomAuthorize(AccountTypeEnum.Admin)]
        [HttpPost]
        public async Task<JsonResult> Delete(short trainingId)
        {
            Result result = await _trainingService.DeleteAsync(trainingId);
            return Json(
                new
                {
                    Success = result.IsSuccess,
                    Message = result.IsSuccess
                    ? "Training deleted successfully"
                    : result.Error.Message ?? "Training cannot be deleted"
                },
                "application/json",
                System.Text.Encoding.UTF8);
        }
    }
}
