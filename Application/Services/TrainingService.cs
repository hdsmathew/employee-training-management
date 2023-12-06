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

        public int Add(Training training)
        {
            if (_trainingRepository.ExistsByName(training.Name))
            {
                throw new Exception("Training already exists.");
            }
            return _trainingRepository.Add(training);
        }

        public void Delete(int trainingID)
        {
            throw new NotImplementedException();
        }

        public void Update(Training training)
        {
            throw new NotImplementedException();
        }
    }
}
