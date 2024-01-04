using Core.Domain;
using System.Collections.Generic;

namespace Core.Application.Repositories
{
    public interface ITrainingRepository
    {
        int Add(Training training);
        int AddWithPrerequisites(Training training);
        int Delete(short trainingID);
        bool ExistsByName(string name);
        Training Get(short trainingID);
        IEnumerable<Training> GetAll();
        IEnumerable<Training> GetAllWithPrerequisites();
        int Update(Training training);
    }
}
