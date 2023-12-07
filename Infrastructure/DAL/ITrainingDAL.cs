using Core.Domain.Training;
using System.Collections.Generic;

namespace Infrastructure.DAL
{
    public interface ITrainingDAL
    {
        int Add(Training training);
        int Delete(int trainingID);
        Training Get(int trainingID);
        bool ExistsByName(string name);
        IEnumerable<Training> GetAll();
        int Update(Training training);
    }
}
