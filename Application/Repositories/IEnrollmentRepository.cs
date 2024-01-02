﻿using Core.Domain;
using System.Collections.Generic;

namespace Core.Application.Repositories
{
    public interface IEnrollmentRepository
    {
        int Add(Enrollment enrollment);
        int AddWithEmployeeUploads(Enrollment enrollment, IEnumerable<EmployeeUpload> employeeUploads);
        int Delete(int enrollmentID);
        bool Exists(short employeeID, short trainingID);
        Enrollment Get(int enrollmentID);
        IEnumerable<Enrollment> GetAll();
        int Update(Enrollment enrollment);
    }
}
