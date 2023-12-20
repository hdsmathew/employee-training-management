using Core.Application.Models;
using Core.Application.Repositories;
using Core.Domain;
using System;

namespace Core.Application.Services
{
    public class TrainingService : ITrainingService
    {
        private readonly ITrainingRepository _trainingRepository;

        public TrainingService(ITrainingRepository trainingRepository)
        {
            _trainingRepository = trainingRepository;
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
                response.AddedRows = _trainingRepository.Add(training);
            }
            catch (Exception ex)
            {
                response.AddError(new ErrorModel()
                {
                    Message = "Unable to add new training. Try again later.",
                    Exception = ex
                });
            }
            return response;
        }

        public ResponseModel<Training> Delete(int trainingId)
        {
            ResponseModel<Training> response = new ResponseModel<Training>();
            try
            {
                response.DeletedRows = _trainingRepository.Delete(trainingId);
            }
            catch (Exception ex)
            {
                response.AddError(new ErrorModel()
                {
                    Message = $"Unable to delete training with Id: {trainingId}",
                    Exception = ex
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
            catch (Exception ex)
            {
                response.AddError(new ErrorModel()
                {
                    Message = $"Unable to update training with Id: {training.TrainingId}",
                    Exception = ex
                });
            }
            return response;
        }
    }
}
