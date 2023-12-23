using Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Common
{
    public abstract class MapperBase<T, E> where E : EntityBase
    {
        public abstract E MapDomainModelToEntity(T domainModel);
        public abstract T MapEntityToDomainModel(E entity);
        public abstract E MapRowToEntity((string, object)[] entityValueTuples);

        public IEnumerable<E> MapDomainModelToEntities(IEnumerable<T> domainModeList)
        {
            List<E> entities = new List<E>();
            foreach (T domainModel in domainModeList)
            {
                entities.Add(MapDomainModelToEntity(domainModel));
            }
            return entities;
        }

        public IEnumerable<T> MapEntitiesToDomainModel(IEnumerable<E> entities)
        {
            List<T> domainModelList = new List<T>();
            foreach(E entity in entities)
            {
                domainModelList.Add(MapEntityToDomainModel(entity));
            }
            return domainModelList;
        }

        public IEnumerable<E> MapTableToEntities(IEnumerable<(string, object)[]> entityValueTuplesArrays)
        {
            List<E> entities = new List<E>();
            foreach ((string, object)[] entityValueTuples in entityValueTuplesArrays)
            {
                entities.Add(MapRowToEntity(entityValueTuples));
            }
            return entities;
        }

        protected S GetValueFromTuple<S>(string fieldName, (string, object)[] entityValueTuples)
        {
            var tuple = entityValueTuples.FirstOrDefault(t => t.Item1 == fieldName);
            return (tuple.Item2 is S castedValue) ? castedValue : default;
        }
    }
}
