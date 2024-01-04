using Core.Application.Models;
using Core.Application.Repositories;
using Core.Domain;
using System.Linq;

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

        public ResponseModel<Training> Add(Training training)
        {
            ResponseModel<Training> response = new ResponseModel<Training>();
            try
            {
                if (_trainingRepository.ExistsByName(training.TrainingName))
                {
                    response.AddError(new ErrorModel()
                    {
                        Message = $"Training with name: {training.TrainingName} already exists."
                    });
                    return response;
                }
                if (training.Prerequisites.Any())
                {
                    response.AddedRows = _trainingRepository.AddWithPrerequisites(training);
                }
                else
                {
                    response.AddedRows = _trainingRepository.Add(training);
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

        public ResponseModel<Training> Delete(short trainingId)
        {
            ResponseModel<Training> response = new ResponseModel<Training>();
            try
            {
                response.DeletedRows = _trainingRepository.Delete(trainingId);
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

        public ResponseModel<TrainingViewModel> GetTrainingViewModel()
        {
            ResponseModel<TrainingViewModel> response = new ResponseModel<TrainingViewModel>();
            try
            {
                response.Entity = new TrainingViewModel
                {
                    Departments = _departmentRepository.GetAll(),
                    Prerequisites = _prerequisiteRepository.GetAll()
                };
            }
            catch (DALException dalEx)
            {
                _logger.LogError(dalEx, "Error in retrieving TrainingViewModel");
                response.AddError(new ErrorModel()
                {
                    Message = $"Unable to retrieve TrainingViewModel",
                    Exception = dalEx
                });
            }
            return response;
        }

        public ResponseModel<TrainingViewModel> GetTrainingViewModel(short trainingId)
        {
            ResponseModel<TrainingViewModel> response = GetTrainingViewModel();
            try
            {
                Training training = _trainingRepository.Get(trainingId);
                response.Entity.PreferredDepartmentId = training.PreferredDepartmentId;
                response.Entity.TrainingDescription = training.TrainingDescription;
                response.Entity.TrainingName = training.TrainingName;
                response.Entity.SeatsAvailable = training.SeatsAvailable;
                response.Entity.RegistrationDeadline = training.RegistrationDeadline;
            }
            catch (DALException dalEx)
            {
                _logger.LogError(dalEx, "Error in retrieving training");
                response.AddError(new ErrorModel()
                {
                    Message = $"Unable to retrieve training with Id: {trainingId}",
                    Exception = dalEx
                });
            }
            return response;
        }

        public ResponseModel<Training> Update(Training training)
        {
            ResponseModel<Training> response = new ResponseModel<Training>();
            try
            {
                response.UpdatedRows = _trainingRepository.Update(training);
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
