using Core.Application.Repositories;
using Core.Domain.Training;
using Infrastructure.DAL;
using System.Collections.Generic;

namespace Infrastructure.Repositories
{
    public class TrainingRepository : ITrainingRepository
    {
        private readonly ITrainingDAL _trainingDAL;

        public TrainingRepository(ITrainingDAL trainingDAL)
        {
            _trainingDAL = trainingDAL;
        }

        public int Add(Training training)
        {
            return _trainingDAL.Add(training);
        }

        public bool Delete(int trainingID)
        {
            return _trainingDAL.Delete(trainingID);
        }

        public bool ExistsByName(string name)
        {
            return _trainingDAL.ExistsByName(name);
        }

        public Training Get(int trainingID)
        {
            return _trainingDAL.Get(trainingID);
        }

        public IEnumerable<Training> GetAll()
        {
            return _trainingDAL.GetAll();
        }

        public bool Update(Training training)
        {
            return _trainingDAL.Update(training);
        }
    }
}
