using System.Data;

namespace Assignment_v1.Common
{
    internal abstract class MapperBase<T>
    {
        public abstract T MapRowToObject(DataRow dataRow);

        public IEnumerable<T> MapTableToObjects(DataTable table)
        {
            List<T> entities = new List<T>();
            foreach (DataRow dataRow in table.Rows)
            {
                T entity = MapRowToObject(dataRow);
                entities.Add(entity);
            }

            return entities;
        }
    }
}
