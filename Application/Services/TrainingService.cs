using Core.Application.Models;
using Core.Application.Repositories;
using Core.Domain;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Application.Services
{
    public class TrainingService : ITrainingService
    {
        private readonly ILogger _logger;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IPrerequisiteRepository _prerequisiteRepository;
        private readonly ITrainingRepository _trainingRepository;

        public TrainingService(ITrainingRepository trainingRepository, ILogger logger, IPrerequisiteRepository prerequisiteRepository, IDepartmentRepository departmentRepository)
        {
            _trainingRepository = trainingRepository;
            _logger = logger;
            _prerequisiteRepository = prerequisiteRepository;
            _departmentRepository = departmentRepository;
        }

        public async Task<ResponseModel<Training>> AddAsync(Training training)
        {
            ResponseModel<Training> response = new ResponseModel<Training>();
            try
            {
                if (await _trainingRepository.ExistsByName(training.TrainingName))
                {
                    response.AddError(new ErrorModel()
                    {
                        Message = $"Training with name: {training.TrainingName} already exists."
                    });
                    return response;
                }
                if (training.Prerequisites.Any())
                {
                    response.AddedRows = await _trainingRepository.AddWithPrerequisites(training);
                }
                else
                {
                    response.AddedRows = await _trainingRepository.Add(training);
                }
            }
            catch (DALException dalEx)
            {
                _logger.LogError(dalEx, "Error in adding training");
                response.AddError(new ErrorModel()
                {
                    Message = "Unable to add new training. Try again later.",
                    Exception = dalEx
                });
            }
            return response;
        }

        public async Task<ResponseModel<Training>> DeleteAsync(short trainingId)
        {
            ResponseModel<Training> response = new ResponseModel<Training>();
            try
            {
                if (await _trainingRepository.HasEnrollments(trainingId))
                {
                    response.AddError(new ErrorModel()
                    {
                        Message = $"Training already has pending or approved enrollments."
                    });
                    return response;
                }
                response.DeletedRows = await _trainingRepository.Delete(trainingId);
            }
            catch (DALException dalEx)
            {
                _logger.LogError(dalEx, "Error in deleting training");
                response.AddError(new ErrorModel()
                {
                    Message = $"Unable to delete training with Id: {trainingId}",
                    Exception = dalEx
                });
            }
            return response;
        }

        public async Task<ResponseModel<TrainingViewModel>> GetTrainingDetailsAsync()
        {
            ResponseModel<TrainingViewModel> response = new ResponseModel<TrainingViewModel>();
            try
            {
                response.Entity = new TrainingViewModel
                {
                    Departments = await _departmentRepository.GetAllAsync(),
                    Prerequisites = await _prerequisiteRepository.GetAllAsync()
                };
            }
            catch (DALException dalEx)
            {
                _logger.LogError(dalEx, "Error in retrieving training details");
                response.AddError(new ErrorModel()
                {
                    Message = "Unable to retrieve training details",
                    Exception = dalEx
                });
            }
            return response;
        }

        public async Task<ResponseModel<TrainingViewModel>> GetTrainingDetailsAsync(short trainingId)
        {
            ResponseModel<TrainingViewModel> response = await GetTrainingDetailsAsync();
            try
            {
                Training training = await _trainingRepository.GetAsync(trainingId);
                response.Entity.PreferredDepartmentId = training.PreferredDepartmentId;
                response.Entity.TrainingDescription = training.TrainingDescription;
                response.Entity.TrainingName = training.TrainingName;
                response.Entity.SeatsAvailable = training.SeatsAvailable;
                response.Entity.RegistrationDeadline = training.RegistrationDeadline;
            }
            catch (DALException dalEx)
            {
                _logger.LogError(dalEx, "Error in retrieving training details");
                response.AddError(new ErrorModel()
                {
                    Message = $"Unable to retrieve training with Id: {trainingId}",
                    Exception = dalEx
                });
            }
            return response;
        }

        public async Task<ResponseModel<Training>> UpdateAsync(Training training)
        {
            ResponseModel<Training> response = new ResponseModel<Training>();
            try
            {
                response.UpdatedRows = await _trainingRepository.Update(training);
            }
            catch (DALException dalEx)
            {
                _logger.LogError(dalEx, "Error in updating training");
                response.AddError(new ErrorModel()
                {
                    Message = $"Unable to update training with Id: {training.TrainingId}",
                    Exception = dalEx
                });
            }
            return response;
        }
    }
}
