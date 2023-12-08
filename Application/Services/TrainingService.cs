using Core.Application.Models;
using Core.Application.Repositories;
using Core.Domain.Training;
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

        public Response<Training> Add(Training training)
        {
            Response<Training> response = new Response<Training>();
            try
            {
                if (_trainingRepository.ExistsByName(training.Name))
                {
                    response.AddError(new Error()
                    {
                        Message = $"Training with name: {training.Name} already exists."
                    });
                    return response;
                }
                response.AddedRows = _trainingRepository.Add(training);
            }
            catch (Exception ex)
            {
                response.AddError(new Error()
                {
                    Message = "Unable to add new training. Try again later.",
                    Exception = ex
                });
            }
            return response;
        }

        public Response<Training> Delete(int trainingID)
        {
            Response<Training> response = new Response<Training>();
            try
            {
                response.DeletedRows = _trainingRepository.Delete(trainingID);
            }
            catch (Exception ex)
            {
                response.AddError(new Error()
                {
                    Message = $"Unable to delete training with Id: {trainingID}",
                    Exception = ex
                });
            }
            return response;
        }

        public Response<Training> Update(Training training)
        {
            Response<Training> response = new Response<Training>();
            try
            {
                response.UpdatedRows = _trainingRepository.Update(training);
            }
            catch (Exception ex)
            {
                response.AddError(new Error()
                {
                    Message = $"Unable to update training with Id: {training.ID}",
                    Exception = ex
                });
            }
            return response;
        }
    }
}
