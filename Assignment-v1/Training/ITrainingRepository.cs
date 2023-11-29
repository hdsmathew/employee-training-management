namespace Assignment_v1.Training
{
    public interface ITrainingRepository
    {
        bool Add(Training training);
        bool Delete(int trainingID);
        Training Get(int trainingID);
        IEnumerable<Training> GetAll();
        bool Update(Training training);
    }
}
