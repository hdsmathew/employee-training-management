using Infrastructure.Models;
using System.Collections.Generic;

namespace Infrastructure.DAL.Interfaces
{
    public interface IPrerequisiteDAL
    {
        int Add(PrerequisiteModel prerequisite);
        IEnumerable<PrerequisiteModel> GetAllByTrainingId(short trainingId);
    }
}
