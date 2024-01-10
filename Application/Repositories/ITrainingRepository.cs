using Core.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Application.Repositories
{
    public interface ITrainingRepository
    {
        Task<int> Add(Training training);
        Task<int> AddWithPrerequisites(Training training);
        Task<int> Delete(short trainingID);
        Task<bool> ExistsByName(string name);
        Task<Training> GetAsync(short trainingID);
        Task<IEnumerable<Training>> GetAllAsync();
        Task<IEnumerable<Training>> GetAllByRegistrationDeadlineDueAsync(DateTime registrationDeadline);
        Task<IEnumerable<Training>> GetAllWithPrerequisitesAsync();
        Task<bool> HasEnrollments(short trainingId);
        Task<int> Update(Training training);
    }
}
