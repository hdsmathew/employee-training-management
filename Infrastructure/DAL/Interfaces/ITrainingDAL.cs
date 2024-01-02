using Infrastructure.Models;
using System.Collections.Generic;

namespace Infrastructure.DAL.Interfaces
{
    public interface ITrainingDAL
    {
        int Add(TrainingModel training);
        int Delete(int trainingID);
        TrainingModel Get(int trainingID);
        bool ExistsByName(string name);
        IEnumerable<TrainingModel> GetAll();
        int Update(TrainingModel training);
    }
}
