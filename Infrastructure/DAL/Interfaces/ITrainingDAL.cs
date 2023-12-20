using Infrastructure.Entities;
using System.Collections.Generic;

namespace Infrastructure.DAL.Interfaces
{
    public interface ITrainingDAL
    {
        int Add(TrainingEntity training);
        int Delete(int trainingID);
        TrainingEntity Get(int trainingID);
        bool ExistsByName(string name);
        IEnumerable<TrainingEntity> GetAll();
        int Update(TrainingEntity training);
    }
}
