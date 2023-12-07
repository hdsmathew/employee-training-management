using System.Collections.Generic;

namespace Core.Domain.Common
{
    public abstract class MapperBase<T>
    {
        public abstract T MapRowToObject(Dictionary<string, object> row);

        public IEnumerable<T> MapTableToObjects(IEnumerable<Dictionary<string, object>> entityDicts)
        {
            List<T> entities = new List<T>();
            foreach (Dictionary<string, object> row in entityDicts)
            {
                entities.Add(MapRowToObject(row));
            }
            return entities;
        }
    }
}
