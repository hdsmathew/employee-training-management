using Infrastructure.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.DAL.Interfaces
{
    public interface IPrerequisiteDAL
    {
        Task AddAsync(PrerequisiteModel prerequisite);
        Task<IEnumerable<PrerequisiteModel>> GetAllAsync();
        Task<IEnumerable<PrerequisiteModel>> GetAllByTrainingIdAsync(short trainingId);
    }
}
