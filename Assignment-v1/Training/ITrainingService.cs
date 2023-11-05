namespace Assignment_v1.Training.Training
{
    internal interface ITrainingService
    {
        void AddTraining(Training training);
        void EditTraining(Training training);
        void DeleteTraining(int trainingID);
    }
}
