using Infrastructure.Models;
using System;
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
        IEnumerable<TrainingModel> GetAllByRegistrationDeadlineDue(DateTime registrationDeadline);
        int Update(TrainingModel training);
        bool HasEnrollments(short trainingId);
    }
}
