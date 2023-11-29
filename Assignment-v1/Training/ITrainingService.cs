namespace Assignment_v1.Training
{
    public interface ITrainingService
    {
        void Add(Training training);
        void Edit(Training training);
        void Delete(int trainingID);
    }
}
