using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.DAL.Interfaces
{
    public interface ITrainingDAL
    {
        Task<int> AddAsync(TrainingModel training);
        Task<int> AddWithPrerequisitesAsync(TrainingModel training, IEnumerable<PrerequisiteModel> prerequisites);
        Task<int> DeleteAsync(short trainingID);
        Task<TrainingModel> GetAsync(short trainingID);
        Task<bool> ExistsByNameAsync(string name);
        Task<IEnumerable<TrainingModel>> GetAllAsync();
        Task<IEnumerable<TrainingModel>> GetAllByRegistrationDeadlineDueAsync(DateTime registrationDeadline);
        Task<int> UpdateAsync(TrainingModel training);
        Task<bool> HasEnrollmentsAsync(short trainingId);
    }
}
