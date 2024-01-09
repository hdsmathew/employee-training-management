using Core.Domain;
using System;
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
        IEnumerable<Training> GetAllByRegistrationDeadlineDue(DateTime registrationDeadline);
        IEnumerable<Training> GetAllWithPrerequisites();
        bool HasEnrollments(short trainingId);
        int Update(Training training);
    }
}
