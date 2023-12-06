using Core.Domain.Training;
using System.Collections.Generic;

namespace Core.Application.Repositories
{
    public interface ITrainingRepository
    {
        int Add(Training training);
        bool Delete(int trainingID);
        bool ExistsByName(string name);
        Training Get(int trainingID);
        IEnumerable<Training> GetAll();
        bool Update(Training training);
    }
}
