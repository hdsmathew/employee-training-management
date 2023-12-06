using Core.Domain.Training;
using System.Collections.Generic;

namespace Infrastructure.DAL
{
    public interface ITrainingDAL
    {
        int Add(Training training);
        bool Delete(int trainingID);
        Training Get(int trainingID);
        bool ExistsByName(string name);
        IEnumerable<Training> GetAll();
        bool Update(Training training);
    }
}
