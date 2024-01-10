using Core.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Application.Repositories
{
    public interface IPrerequisiteRepository
    {
        Task<IEnumerable<Prerequisite>> GetAllAsync();
    }
}
