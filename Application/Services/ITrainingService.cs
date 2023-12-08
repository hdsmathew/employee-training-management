using Core.Application.Models;
using Core.Domain.Training;

namespace Core.Application.Services
{
    public interface ITrainingService
    {
        Response<Training> Add(Training training);
        Response<Training> Update(Training training);
        Response<Training> Delete(int trainingID);
    }
}
