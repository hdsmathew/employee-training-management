using Core.Application.Models;
using Core.Domain;
using System.Threading.Tasks;

namespace Core.Application.Services
{
    public interface ITrainingService
    {
        Task<Result> AddAsync(Training training);
        Task<Result> UpdateAsync(Training training);
        Task<Result> DeleteAsync(short trainingID);
        Task<ResultT<TrainingViewModel>> GetTrainingDetailsAsync();
        Task<ResultT<TrainingViewModel>> GetTrainingDetailsAsync(short trainingId);
    }
}
