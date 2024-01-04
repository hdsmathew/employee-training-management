using Core.Domain;
using System.Collections.Generic;

namespace Core.Application.Repositories
{
    public interface IPrerequisiteRepository
    {
        IEnumerable<Prerequisite> GetAll();
    }
}
