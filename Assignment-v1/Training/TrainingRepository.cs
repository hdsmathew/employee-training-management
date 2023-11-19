namespace Assignment_v1.Training
{
    internal class TrainingRepository : ITrainingRepository
    {
        private readonly ITrainingDAL _trainingDAL;

        public TrainingRepository(ITrainingDAL trainingDAL)
        {
            _trainingDAL = trainingDAL;
        }

        public void Add(Training training)
        {
            throw new NotImplementedException();
        }

        public void Delete(int trainingID)
        {
            throw new NotImplementedException();
        }

        public Training Get(int trainingID)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Training> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Update(Training training)
        {
            throw new NotImplementedException();
        }
    }
}
