using Infrastructure.Models;
using System.Collections.Generic;

namespace Infrastructure.DAL.Interfaces
{
    public interface ITrainingDAL
    {
        int Add(TrainingModel training);
        int AddWithPrerequisites(TrainingModel training, IEnumerable<PrerequisiteModel> prerequisites);
        int Delete(short trainingID);
        TrainingModel Get(short trainingID);
        bool ExistsByName(string name);
        IEnumerable<TrainingModel> GetAll();
        int Update(TrainingModel training);
    }
}
