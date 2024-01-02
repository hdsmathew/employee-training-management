﻿using Core.Domain;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Common
{
    public abstract class MapperBase<TEntity, TModel>
        where TEntity : IEntity
        where TModel : IModel
    {
        public abstract TModel MapEntityToDataModel(TEntity entity);
        public abstract TEntity MapDataModelToEntity(TModel model);
        public abstract TModel MapRowToDataModel((string, object)[] entityValueTuples);

        public IEnumerable<TModel> MapEntitiesToDataModels(IEnumerable<TEntity> entities)
        {
            List<TModel> models = new List<TModel>();
            foreach (TEntity entity in entities)
            {
                models.Add(MapEntityToDataModel(entity));
            }
            return models;
        }

        public IEnumerable<TEntity> MapDataModelsToEntities(IEnumerable<TModel> models)
        {
            List<TEntity> entities = new List<TEntity>();
            foreach (TModel model in models)
            {
                entities.Add(MapDataModelToEntity(model));
            }
            return entities;
        }

        public IEnumerable<TModel> MapTableToEntities(IEnumerable<(string, object)[]> entityValueTuplesArrays)
        {
            List<TModel> entities = new List<TModel>();
            foreach ((string, object)[] entityValueTuples in entityValueTuplesArrays)
            {
                entities.Add(MapRowToDataModel(entityValueTuples));
            }
            return entities;
        }

        protected TValue GetValueFromTuple<TValue>(string fieldName, (string, object)[] entityValueTuples)
        {
            var tuple = entityValueTuples.FirstOrDefault(t => t.Item1 == fieldName);
            if (tuple.Item2 == null)
            {
                return default;
            }

            try
            {
                return (TValue)Convert.ChangeType(tuple.Item2, typeof(TValue));
            }
            catch (InvalidCastException ex)
            {
                throw new InvalidOperationException($"Error converting value for fieldName {fieldName} to type {typeof(TValue)}", ex);
            }
        }
    }
}
