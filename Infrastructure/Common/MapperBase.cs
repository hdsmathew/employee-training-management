using Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Common
{
    public abstract class MapperBase<TDomain, TEntity> where TEntity : EntityBase
    {
        public abstract TEntity MapDomainModelToEntity(TDomain domainModel);
        public abstract TDomain MapEntityToDomainModel(TEntity entity);
        public abstract TEntity MapRowToEntity((string, object)[] entityValueTuples);

        public IEnumerable<TEntity> MapDomainModelToEntities(IEnumerable<TDomain> domainModeList)
        {
            List<TEntity> entities = new List<TEntity>();
            foreach (TDomain domainModel in domainModeList)
            {
                entities.Add(MapDomainModelToEntity(domainModel));
            }
            return entities;
        }

        public IEnumerable<TDomain> MapEntitiesToDomainModel(IEnumerable<TEntity> entities)
        {
            List<TDomain> domainModelList = new List<TDomain>();
            foreach (TEntity entity in entities)
            {
                domainModelList.Add(MapEntityToDomainModel(entity));
            }
            return domainModelList;
        }

        public IEnumerable<TEntity> MapTableToEntities(IEnumerable<(string, object)[]> entityValueTuplesArrays)
        {
            List<TEntity> entities = new List<TEntity>();
            foreach ((string, object)[] entityValueTuples in entityValueTuplesArrays)
            {
                entities.Add(MapRowToEntity(entityValueTuples));
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
