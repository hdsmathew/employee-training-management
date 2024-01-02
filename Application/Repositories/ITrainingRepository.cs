using Core.Domain;
using System.Collections.Generic;

namespace Core.Application.Repositories
{
    public interface ITrainingRepository
    {
        int Add(Training training);
        int Delete(int trainingID);
        bool ExistsByName(string name);
        Training Get(int trainingID);
        IEnumerable<Training> GetAll();
        IEnumerable<Training> GetAllWithPrerequisites();
        int Update(Training training);
    }
}
