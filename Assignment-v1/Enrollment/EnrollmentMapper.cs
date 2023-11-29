using Assignment_v1.Common;
using System;
using System.Collections.Generic;

namespace Assignment_v1.Enrollment
{
    public class EnrollmentMapper : MapperBase<Enrollment>
    {
        public override Enrollment MapRowToObject(Dictionary<string, object> row)
        {
            Enrollment enrollment = new Enrollment()
            {
                ID = Convert.ToInt32(row["ID"]),
                EmployeeID = Convert.ToInt32(row["employeeID"]),
                TrainingID = Convert.ToInt32(row["trainingID"]),
                Status = (EnrollmentStatusEnum)row["status"],
                Message = row["message"].ToString(),
                RequestDate = Convert.ToDateTime(row["requestDate"]),
                ResponseDate = Convert.ToDateTime(row["responseDate"])
            };

            return enrollment;
        }
    }
}
