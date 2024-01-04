using Core.Application.Repositories;
using Core.Domain;
using Infrastructure.Common;
using Infrastructure.DAL;
using Infrastructure.DAL.Interfaces;
using Infrastructure.Models;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Repositories
{
    public class TrainingRepository : ITrainingRepository
    {
        private readonly IPrerequisiteDAL _prerequisiteDAL;
        private readonly ITrainingDAL _trainingDAL;
        private readonly MapperBase<Prerequisite, PrerequisiteModel> _prerequisiteMapper;
        private readonly MapperBase<Training, TrainingModel> _trainingMapper;

        public TrainingRepository(IPrerequisiteDAL prerequisiteDAL, PrerequisiteMapper prerequisiteMapper, ITrainingDAL trainingDAL, TrainingMapper trainingMapper)
        {
            _prerequisiteDAL = prerequisiteDAL;
            _prerequisiteMapper = prerequisiteMapper;
            _trainingDAL = trainingDAL;
            _trainingMapper = trainingMapper;
        }

        public int Add(Training training)
        {
            TrainingModel trainingEntity = _trainingMapper.MapEntityToDataModel(training);
            return _trainingDAL.Add(trainingEntity);
        }

        public int AddWithPrerequisites(Training training)
        {
            TrainingModel trainingEntity = _trainingMapper.MapEntityToDataModel(training);
            IEnumerable<PrerequisiteModel> prerequisiteModels = _prerequisiteMapper.MapEntitiesToDataModels(training.Prerequisites);
            return _trainingDAL.AddWithPrerequisites(trainingEntity, prerequisiteModels);
        }

        public int Delete(short trainingID)
        {
            return _trainingDAL.Delete(trainingID);
        }

        public bool ExistsByName(string name)
        {
            return _trainingDAL.ExistsByName(name);
        }

        public Training Get(short trainingID)
        {
            TrainingModel trainingEntity = _trainingDAL.Get(trainingID);
            return _trainingMapper.MapDataModelToEntity(trainingEntity);
        }

        public Training GetWithPrerequisites(short trainingID)
        {
            Training training = Get(trainingID);
            training.SetPrerequisites(_prerequisiteMapper.MapDataModelsToEntities(_prerequisiteDAL.GetAllByTrainingId(trainingID)));
            return training;
        }

        public IEnumerable<Training> GetAll()
        {
            IEnumerable<TrainingModel> trainingEntities = _trainingDAL.GetAll();
            return _trainingMapper.MapDataModelsToEntities(trainingEntities);
        }

        public IEnumerable<Training> GetAllWithPrerequisites()
        {
            List<Training> trainings = GetAll().ToList();
            trainings.ForEach(t => t.SetPrerequisites(_prerequisiteMapper.MapDataModelsToEntities(_prerequisiteDAL.GetAllByTrainingId(t.TrainingId))));
            return trainings;
        }

        public int Update(Training training)
        {
            TrainingModel trainingEntity = _trainingMapper.MapEntityToDataModel(training);
            return _trainingDAL.Update(trainingEntity);
        }
    }
}
