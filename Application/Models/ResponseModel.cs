using System.Collections.Generic;

namespace Core.Application.Models
{
    public class ResponseModel<T>
    {
        private readonly List<ErrorModel> _errors;

        public ResponseModel()
        {
            _errors = new List<ErrorModel>();
        }

        public IEnumerable<T> Entities { get; set; }
        public T Entity { get; set; }
        public int AddedRows {  get; set; }
        public int DeletedRows { get; set; }
        public int UpdatedRows { get; set; }

        public void AddError(ErrorModel error)
        {
            _errors.Add(error);
        }

        public IEnumerable<ErrorModel> GetErrors()
        {
            return _errors;
        }

        public bool Success()
        {
            return _errors.Count == 0;
        }

        public bool Failure()
        {
            return !Success();
        }
    }
}
