using System.Collections.Generic;

namespace Assignment_v1.Training
{
    public class TrainingRepository : ITrainingRepository
    {
        private readonly ITrainingDAL _trainingDAL;

        public TrainingRepository(ITrainingDAL trainingDAL)
        {
            _trainingDAL = trainingDAL;
        }

        public bool Add(Training training)
        {
            return _trainingDAL.Add(training);
        }

        public bool Delete(int trainingID)
        {
            return _trainingDAL.Delete(trainingID);
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
