using Infrastructure.Entities;
using System.Collections.Generic;

namespace Infrastructure.Common
{
    public abstract class MapperBase<T, E> where E : EntityBase
    {
        public abstract E MapDomainModelToEntity(T domainModel);
        public abstract T MapEntityToDomainModel(E entity);
        public abstract E MapRowToEntity(Dictionary<string, object> row);

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
        public IEnumerable<E> MapTableToEntities(IEnumerable<Dictionary<string, object>> entityDicts)
        {
            List<E> entities = new List<E>();
            foreach (Dictionary<string, object> row in entityDicts)
            {
                entities.Add(MapRowToEntity(row));
            }
            return entities;
        }
    }
}
