using Assignment_v1.Common;
using System.Data;

namespace Assignment_v1.Enrollment
{
    internal class EnrollmentMapper : MapperBase<Enrollment>
    {
        public override Enrollment MapRowToObject(DataRow dataRow)
        {
            Enrollment enrollment = new Enrollment()
            {
                ID = Convert.ToInt32(dataRow["ID"]),
                EmployeeID = Convert.ToInt32(dataRow["employeeID"]),
                TrainingID = Convert.ToInt32(dataRow["trainingID"]),
                Status = (EnrollmentStatusEnum)dataRow["status"],
                Message = dataRow["message"].ToString(),
                RequestDate = Convert.ToDateTime(dataRow["requestDate"]),
                ResponseDate = Convert.ToDateTime(dataRow["responseDate"])
            };

            return enrollment;
        }
    }
}
