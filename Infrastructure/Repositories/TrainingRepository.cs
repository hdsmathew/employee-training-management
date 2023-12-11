using Core.Application.Repositories;
using Core.Domain.Training;
using Infrastructure.Common;
using Infrastructure.DAL;
using Infrastructure.Entities;
using System.Collections.Generic;

namespace Infrastructure.Repositories
{
    public class TrainingRepository : ITrainingRepository
    {
        private readonly ITrainingDAL _trainingDAL;
        private readonly MapperBase<Training, TrainingEntity> _trainingMapper;

        public TrainingRepository(ITrainingDAL trainingDAL, TrainingMapper trainingMapper)
        {
            _trainingDAL = trainingDAL;
            _trainingMapper = trainingMapper;
        }

        public int Add(Training training)
        {
            TrainingEntity trainingEntity = _trainingMapper.MapDomainModelToEntity(training);
            return _trainingDAL.Add(trainingEntity);
        }

        public int Delete(int trainingID)
        {
            return _trainingDAL.Delete(trainingID);
        }

        public bool ExistsByName(string name)
        {
            return _trainingDAL.ExistsByName(name);
        }

        public Training Get(int trainingID)
        {
            TrainingEntity trainingEntity = _trainingDAL.Get(trainingID);
            return _trainingMapper.MapEntityToDomainModel(trainingEntity);
        }

        public IEnumerable<Training> GetAll()
        {
            IEnumerable<TrainingEntity> trainingEntities = _trainingDAL.GetAll();
            return _trainingMapper.MapEntitiesToDomainModel(trainingEntities);
        }

        public int Update(Training training)
        {
            TrainingEntity trainingEntity = _trainingMapper.MapDomainModelToEntity(training);
            return _trainingDAL.Update(trainingEntity);
        }
    }
}
