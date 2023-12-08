using System.Collections.Generic;

namespace Core.Application.Models
{
    public class Response<T>
    {
        private readonly List<Error> _errors;

        public Response()
        {
            _errors = new List<Error>();
        }

        public List<T> Entities;
        public T Entity { get; set; }
        public int AddedRows {  get; set; }
        public int DeletedRows { get; set; }
        public int UpdatedRows { get; set; }

        public void AddError(Error error)
        {
            _errors.Add(error);
        }

        public List<Error> GetErrors()
        {
            return _errors;
        }

        public bool HasErrors()
        {
            return _errors.Count > 0;
        }
    }
}
