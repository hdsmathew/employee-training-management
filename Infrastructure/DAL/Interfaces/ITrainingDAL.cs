using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.DAL.Interfaces
{
    public interface ITrainingDAL
    {
        Task AddAsync(TrainingModel training);
        Task AddWithPrerequisitesAsync(TrainingModel training, IEnumerable<PrerequisiteModel> prerequisites);
        Task DeleteAsync(short trainingId);
        Task<TrainingModel> GetAsync(short trainingId);
        Task<bool> ExistsByNameAsync(string name);
        Task<IEnumerable<TrainingModel>> GetAllAsync();
        Task<IEnumerable<TrainingModel>> GetAllByRegistrationDeadlineDueAsync(DateTime registrationDeadline);
        Task UpdateAsync(TrainingModel training);
        Task<bool> HasEnrollmentsAsync(short trainingId);
    }
}
