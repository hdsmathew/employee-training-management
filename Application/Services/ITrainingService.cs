using Core.Domain.Training;

namespace Core.Application.Services
{
    public interface ITrainingService
    {
        int Add(Training training);
        void Update(Training training);
        void Delete(int trainingID);
    }
}
