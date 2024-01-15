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

        public async Task<Result> AddAsync(Training training)
        {
            try
            {
                if (await _trainingRepository.ExistsByName(training.TrainingName))
                {
                    return Result.Failure(new Error($"Training with name: {training.TrainingName} already exists."));
                }

                if (training.Prerequisites.Any())
                {

                    await _trainingRepository.AddWithPrerequisites(training);
                }
                else
                {
                    await _trainingRepository.Add(training);
                }

                return Result.Success();
            }
            catch (DALException dalEx)
            {
                _logger.LogError(dalEx, "Error in adding training");
            }
            catch (MapperException mapperEx)
            {
                _logger.LogError(mapperEx, "Error in mapper");
            }

            return Result.Failure(new Error("Unable to add new training. Try again later."));
        }

        public async Task<Result> DeleteAsync(short trainingId)
        {
            try
            {
                if (await _trainingRepository.HasEnrollments(trainingId))
                {
                    return Result.Failure(new Error("Training already has pending or approved enrollments."));
                }

                await _trainingRepository.Delete(trainingId);

                return Result.Success();
            }
            catch (DALException dalEx)
            {
                _logger.LogError(dalEx, "Error in deleting training");
            }
            catch (MapperException mapperEx)
            {
                _logger.LogError(mapperEx, "Error in mapper");
            }

            return Result.Failure(new Error($"Unable to delete training with Id: {trainingId}"));
        }

        public async Task<ResultT<TrainingViewModel>> GetTrainingDetailsAsync()
        {
            try
            {
                TrainingViewModel trainingViewModel = new TrainingViewModel
                {
                    Departments = await _departmentRepository.GetAllAsync(),
                    Prerequisites = await _prerequisiteRepository.GetAllAsync()
                };

                return ResultT<TrainingViewModel>.Success(trainingViewModel);
            }
            catch (DALException dalEx)
            {
                _logger.LogError(dalEx, "Error in retrieving training details");
            }
            catch (MapperException mapperEx)
            {
                _logger.LogError(mapperEx, "Error in mapper");
            }

            return ResultT<TrainingViewModel>.Failure(new Error("Error in retrieving training details"));
        }

        public async Task<ResultT<TrainingViewModel>> GetTrainingDetailsAsync(short trainingId)
        {
            ResultT<TrainingViewModel> result = await GetTrainingDetailsAsync();
            if (result.IsFailure) return result;

            try
            {
                Training training = await _trainingRepository.GetAsync(trainingId);

                if (training is null) return ResultT<TrainingViewModel>.Failure(new Error("Details for training not found."));

                TrainingViewModel trainingViewModel = result.Value;
                trainingViewModel.PreferredDepartmentId = training.PreferredDepartmentId;
                trainingViewModel.TrainingDescription = training.TrainingDescription;
                trainingViewModel.TrainingName = training.TrainingName;
                trainingViewModel.SeatsAvailable = training.SeatsAvailable;
                trainingViewModel.RegistrationDeadline = training.RegistrationDeadline;

                return ResultT<TrainingViewModel>.Success(trainingViewModel);
            }
            catch (DALException dalEx)
            {
                _logger.LogError(dalEx, "Error in retrieving training details");
            }
            catch (MapperException mapperEx)
            {
                _logger.LogError(mapperEx, "Error in mapper");
            }

            return ResultT<TrainingViewModel>.Failure(new Error($"Unable to retrieve training with Id: {trainingId}"));
        }

        public async Task<Result> UpdateAsync(Training training)
        {
            try
            {
                await _trainingRepository.Update(training);

                return Result.Success();
            }
            catch (DALException dalEx)
            {
                _logger.LogError(dalEx, "Error in updating training");
            }
            catch (MapperException mapperEx)
            {
                _logger.LogError(mapperEx, "Error in mapper");
            }

            return ResultT<TrainingViewModel>.Failure(new Error($"Unable to update training with Id: {training.TrainingId}"));
        }
    }
}
