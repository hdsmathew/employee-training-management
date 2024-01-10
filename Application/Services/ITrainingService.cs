using Core.Application.Models;
using Core.Domain;
using System.Threading.Tasks;

namespace Core.Application.Services
{
    public interface ITrainingService
    {
        Task<ResponseModel<Training>> AddAsync(Training training);
        Task<ResponseModel<Training>> UpdateAsync(Training training);
        Task<ResponseModel<Training>> DeleteAsync(short trainingID);
        Task<ResponseModel<TrainingViewModel>> GetTrainingDetailsAsync();
        Task<ResponseModel<TrainingViewModel>> GetTrainingDetailsAsync(short trainingId);
    }
}
