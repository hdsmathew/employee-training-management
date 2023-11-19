namespace Assignment_v1.Training
{
    internal interface ITrainingRepository
    {
        void Add(Training training);
        void Delete(int trainingID);
        Training Get(int trainingID);
        IEnumerable<Training> GetAll();
        void Update(Training training);
    }
}
