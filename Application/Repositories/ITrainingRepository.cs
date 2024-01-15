using Core.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Application.Repositories
{
    public interface ITrainingRepository
    {
        Task Add(Training training);
        Task AddWithPrerequisites(Training training);
        Task Delete(short trainingID);
        Task<bool> ExistsByName(string name);
        Task<Training> GetAsync(short trainingID);
        Task<IEnumerable<Training>> GetAllAsync();
        Task<IEnumerable<Training>> GetAllByRegistrationDeadlineDueAsync(DateTime registrationDeadline);
        Task<IEnumerable<Training>> GetAllWithPrerequisitesAsync();
        Task<bool> HasEnrollments(short trainingId);
        Task Update(Training training);
    }
}
