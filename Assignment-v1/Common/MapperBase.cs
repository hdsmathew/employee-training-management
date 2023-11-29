using System.Collections.Generic;

namespace Assignment_v1.Common
{
    public abstract class MapperBase<T>
    {
        public abstract T MapRowToObject(Dictionary<string, object> row);

        public IEnumerable<T> MapTableToObjects(IEnumerable<Dictionary<string, object>> entityTable)
        {
            List<T> entities = new List<T>();
            foreach (Dictionary<string, object> row in entityTable)
            {
                T entity = MapRowToObject(row);
                entities.Add(entity);
            }

            return entities;
        }
    }
}
