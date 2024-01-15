using Core.Application.Repositories;
using Core.Domain;
using Infrastructure.Common;
using Infrastructure.DAL.Interfaces;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        public Task Add(Training training)
        {
            TrainingModel trainingEntity = _trainingMapper.MapEntityToDataModel(training);
            return _trainingDAL.AddAsync(trainingEntity);
        }

        public Task AddWithPrerequisites(Training training)
        {
            TrainingModel trainingEntity = _trainingMapper.MapEntityToDataModel(training);
            IEnumerable<PrerequisiteModel> prerequisiteModels = _prerequisiteMapper.MapEntitiesToDataModels(training.Prerequisites);
            return _trainingDAL.AddWithPrerequisitesAsync(trainingEntity, prerequisiteModels);
        }

        public Task Delete(short trainingID)
        {
            return _trainingDAL.DeleteAsync(trainingID);
        }

        public Task<bool> ExistsByName(string name)
        {
            return _trainingDAL.ExistsByNameAsync(name);
        }

        public async Task<Training> GetAsync(short trainingId)
        {
            TrainingModel trainingModel = await _trainingDAL.GetAsync(trainingId);
            return _trainingMapper.MapDataModelToEntity(trainingModel);
        }

        public async Task<Training> GetWithPrerequisitesAsync(short trainingId)
        {
            Training training = await GetAsync(trainingId);
            if (training is null) return null;

            training.SetPrerequisites(_prerequisiteMapper.MapDataModelsToEntities(await _prerequisiteDAL.GetAllByTrainingIdAsync(trainingId)));
            return training;
        }

        public async Task<IEnumerable<Training>> GetAllAsync()
        {
            IEnumerable<TrainingModel> trainingModels = await _trainingDAL.GetAllAsync();
            return _trainingMapper.MapDataModelsToEntities(trainingModels);
        }

        public async Task<IEnumerable<Training>> GetAllByRegistrationDeadlineDueAsync(DateTime registrationDeadline)
        {
            IEnumerable<TrainingModel> trainingModels = await _trainingDAL.GetAllByRegistrationDeadlineDueAsync(registrationDeadline);
            return _trainingMapper.MapDataModelsToEntities(trainingModels);
        }

        public async Task<IEnumerable<Training>> GetAllWithPrerequisitesAsync()
        {
            IEnumerable<Training> trainings = await GetAllAsync();
            if (trainings is null) return null;

            foreach (Training training in trainings)
            {
                training.SetPrerequisites(_prerequisiteMapper.MapDataModelsToEntities(await _prerequisiteDAL.GetAllByTrainingIdAsync(training.TrainingId)));
            }
            return trainings;
        }

        public Task<bool> HasEnrollments(short trainingId)
        {
            return _trainingDAL.HasEnrollmentsAsync(trainingId);
        }

        public Task Update(Training training)
        {
            TrainingModel trainingEntity = _trainingMapper.MapEntityToDataModel(training);
            return _trainingDAL.UpdateAsync(trainingEntity);
        }
    }
}
