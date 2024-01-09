using Core.Application.Repositories;
using Core.Domain;
using Infrastructure.Common;
using Infrastructure.DAL.Interfaces;
using Infrastructure.Models;
using System;
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

        public Training Get(short trainingId)
        {
            TrainingModel trainingModel = _trainingDAL.Get(trainingId);
            return _trainingMapper.MapDataModelToEntity(trainingModel);
        }

        public Training GetWithPrerequisites(short trainingId)
        {
            Training training = Get(trainingId);
            training.SetPrerequisites(_prerequisiteMapper.MapDataModelsToEntities(_prerequisiteDAL.GetAllByTrainingId(trainingId)));
            return training;
        }

        public IEnumerable<Training> GetAll()
        {
            IEnumerable<TrainingModel> trainingModels = _trainingDAL.GetAll();
            return _trainingMapper.MapDataModelsToEntities(trainingModels);
        }

        public IEnumerable<Training> GetAllByRegistrationDeadlineDue(DateTime registrationDeadline)
        {
            IEnumerable<TrainingModel> trainingModels = _trainingDAL.GetAllByRegistrationDeadlineDue(registrationDeadline);
            return _trainingMapper.MapDataModelsToEntities(trainingModels);
        }

        public IEnumerable<Training> GetAllWithPrerequisites()
        {
            List<Training> trainings = GetAll().ToList();
            trainings.ForEach(t => t.SetPrerequisites(_prerequisiteMapper.MapDataModelsToEntities(_prerequisiteDAL.GetAllByTrainingId(t.TrainingId))));
            return trainings;
        }

        public bool HasEnrollments(short trainingId)
        {
            return _trainingDAL.HasEnrollments(trainingId);
        }

        public int Update(Training training)
        {
            TrainingModel trainingEntity = _trainingMapper.MapEntityToDataModel(training);
            return _trainingDAL.Update(trainingEntity);
        }
    }
}
