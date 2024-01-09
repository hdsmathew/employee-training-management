using Core.Application.Models;
using Core.Application.Repositories;
using Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Application.Services
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IEnrollmentRepository _enrollmentRepository;
        private readonly IEnrollmentNotificationRepository _enrollmentNotificationRepository;
        private readonly ILogger _logger;
        private readonly ITrainingRepository _trainingRepository;

        public EnrollmentService(IEnrollmentRepository enrollmentRepository, IEmployeeRepository employeeRepository, ILogger logger, ITrainingRepository trainingRepository, IAccountRepository accountRepository, IEnrollmentNotificationRepository enrollmentNotificationRepository)
        {
            _enrollmentRepository = enrollmentRepository;
            _employeeRepository = employeeRepository;
            _logger = logger;
            _trainingRepository = trainingRepository;
            _accountRepository = accountRepository;
            _enrollmentNotificationRepository = enrollmentNotificationRepository;
        }

        public ResponseModel<EnrollmentViewModel> Approve(int enrollmentId, short approverAccountId)
        {
            ResponseModel<EnrollmentViewModel> response = new ResponseModel<EnrollmentViewModel>();
            try
            {
                Enrollment enrollment = _enrollmentRepository.Get(enrollmentId);
                Employee approver = new Employee() { AccountId = approverAccountId };
                approver.ApproveEnrollment(enrollment);
                response.UpdatedRows = _enrollmentRepository.Update(enrollment);
            }
            catch (DALException dalEx)
            {
                _logger.LogError(dalEx, "Error in processing enrollment");
                response.AddError(new ErrorModel()
                {
                    Message = "Unable to process enrollment. Try again later.",
                    Exception = dalEx
                });
            }
            return response;
        }

        public ResponseModel<EnrollmentViewModel> Decline(DeclineEnrollmentViewModel declineEnrollmentViewModel, short approverAccountId)
        {
            ResponseModel<EnrollmentViewModel> response = new ResponseModel<EnrollmentViewModel>();
            try
            {
                Enrollment enrollment = _enrollmentRepository.Get(declineEnrollmentViewModel.EnrollmentId);
                Employee approver = new Employee() { AccountId = approverAccountId };
                approver.DeclineEnrollment(enrollment);
                response.UpdatedRows = _enrollmentRepository.Update(enrollment);
                // Assume both enrollment and notification inserted
                Training training = _trainingRepository.Get(enrollment.TrainingId);
                _enrollmentNotificationRepository.Add(new EnrollmentNotification(
                    $"Your enrollment application for training: {training.TrainingName} has been declined. Reason: {declineEnrollmentViewModel.ReasonMessage}",
                    enrollment.EmployeeId)
                );
            }
            catch (DALException dalEx)
            {
                _logger.LogError(dalEx, "Error in processing enrollment");
                response.AddError(new ErrorModel()
                {
                    Message = "Unable to process enrollment. Try again later.",
                    Exception = dalEx
                });
            }
            return response;
        }

        public ResponseModel<EnrollmentViewModel> GetEnrollmentSubmissionsForApproval(short managerId)
        {
            ResponseModel<EnrollmentViewModel> response = new ResponseModel<EnrollmentViewModel>();
            try
            {
                response.Entities = _enrollmentRepository.GetAllByManagerIdAndApprovalStatus(
                    managerId,
                    new List<ApprovalStatusEnum>() {
                        ApprovalStatusEnum.Pending
                    }).Select(enrollment => new EnrollmentViewModel(enrollment)
                    {
                        EmployeeName = _employeeRepository.Get(enrollment.EmployeeId)?.GetFullName(),
                        TrainingName = _trainingRepository.Get(enrollment.TrainingId)?.TrainingName
                    });
            }
            catch (DALException dalEx)
            {
                _logger.LogError(dalEx, "Error in retrieving enrollments");
                response.AddError(new ErrorModel()
                {
                    Message = "Unable to retrieve enrollments.",
                    Exception = dalEx
                });
            }
            return response;
        }

        public ResponseModel<EnrollmentViewModel> GetEnrollments(short employeeId)
        {
            ResponseModel<EnrollmentViewModel> response = new ResponseModel<EnrollmentViewModel>();
            try
            {
                response.Entities = _enrollmentRepository.GetAllByEmployeeIdAndApprovalStatus(
                    employeeId,
                    new List<ApprovalStatusEnum>() {
                        ApprovalStatusEnum.Pending, ApprovalStatusEnum.Approved, ApprovalStatusEnum.Declined, ApprovalStatusEnum.Confirmed})
                    .Select(enrollment => new EnrollmentViewModel(enrollment)
                    {
                        TrainingName = _trainingRepository.Get(enrollment.TrainingId)?.TrainingName
                    });
            }
            catch (DALException dalEx)
            {
                _logger.LogError(dalEx, "Error in retrieving enrollments");
                response.AddError(new ErrorModel()
                {
                    Message = "Unable to retrieve enrollments.",
                    Exception = dalEx
                });
            }
            return response;
        }

        public ResponseModel<Enrollment> Submit(short employeeId, short trainingId)
        {
            ResponseModel<Enrollment> response = new ResponseModel<Enrollment>();
            try
            {
                if (_enrollmentRepository.Exists(employeeId, trainingId))
                {
                    response.AddError(new ErrorModel()
                    {
                        Message = $"User already has a pending enrollment submission."
                    });
                    return response;
                }
                response.AddedRows = _enrollmentRepository.Add(new Enrollment()
                {
                    ApprovalStatus = ApprovalStatusEnum.Pending,
                    EmployeeId = employeeId,
                    TrainingId = trainingId
                });
                // Assume both enrollment and notification inserted
                Training training = _trainingRepository.Get(trainingId);
                Employee employee = _employeeRepository.Get(employeeId);
                _enrollmentNotificationRepository.Add(new EnrollmentNotification(
                    $"{employee.GetFullName()} has submitted an enrollment application for training: {training.TrainingName}",
                    employeeId)
                );
            }
            catch (DALException dalEx)
            {
                _logger.LogError(dalEx, "Error in submitting enrollment application");
                response.AddError(new ErrorModel()
                {
                    Message = "Training registration failed. Try again later.",
                    Exception = dalEx
                });
            }
            return response;
        }

        public ResponseModel<Enrollment> Submit(short employeeId, short trainingId, IEnumerable<EmployeeUpload> employeeUploads)
        {
            ResponseModel<Enrollment> response = new ResponseModel<Enrollment>();
            try
            {
                if (_enrollmentRepository.Exists(employeeId, trainingId))
                {
                    response.AddError(new ErrorModel()
                    {
                        Message = $"User already has a pending enrollment submission."
                    });
                    return response;
                }
                response.AddedRows = _enrollmentRepository.AddWithEmployeeUploads(new Enrollment()
                {
                    ApprovalStatus = ApprovalStatusEnum.Pending,
                    EmployeeId = employeeId,
                    TrainingId = trainingId
                }, employeeUploads);
                // Assume both enrollment and notification inserted
                Training training = _trainingRepository.Get(trainingId);
                Employee employee = _employeeRepository.Get(employeeId);
                _enrollmentNotificationRepository.Add(new EnrollmentNotification(
                    $"{employee.GetFullName()} has submitted an enrollment application for training: {training.TrainingName}",
                    employeeId)
                );
            }
            catch (DALException dalEx)
            {
                _logger.LogError(dalEx, "Error in submitting enrollment application");
                response.AddError(new ErrorModel()
                {
                    Message = "Training registration failed. Try again later.",
                    Exception = dalEx
                });
            }
            return response;
        }

        public ResponseModel<ResponseModel<Enrollment>> ValidateApprovedEnrollments(short? approverAccountId)
        {
            ResponseModel<ResponseModel<Enrollment>> response = new ResponseModel<ResponseModel<Enrollment>>();
            try
            {
                short accountId = approverAccountId ?? _accountRepository.GetAccountIdByAccountType(AccountTypeEnum.SysAdmin);
                response.Entities = _trainingRepository.GetAllByRegistrationDeadlineDue(DateTime.Now)
                    .Select(training => ValidateApprovedEnrollmentsByTraining(accountId, training))
                    .ToList();
            }
            catch (DALException dalEx)
            {
                _logger.LogError(dalEx, "Error in retrieving trainings with due registration deadline");
                response.AddError(new ErrorModel()
                {
                    Message = "Unable to validate approved enrollment applications. Try again later.",
                    Exception = dalEx
                });
            }
            return response;
        }

        public ResponseModel<Enrollment> ValidateApprovedEnrollmentsByTraining(short approverAccountId, short trainingId)
        {
            ResponseModel<Enrollment> response = new ResponseModel<Enrollment>();
            try
            {
                Training training = _trainingRepository.Get(trainingId);
                response = ValidateApprovedEnrollmentsByTraining(approverAccountId, training);
            }
            catch (DALException dalEx)
            {
                _logger.LogError(dalEx, $"Error in retrieving training with id {trainingId}");
                response.AddError(new ErrorModel()
                {
                    Message = "Unable to validate approved enrollment applications for training. Try again later.",
                    Exception = dalEx
                });
            }
            return response;
        }

        private ResponseModel<Enrollment> ValidateApprovedEnrollmentsByTraining(short? approverAccountId, Training training)
        {
            ResponseModel<Enrollment> response = new ResponseModel<Enrollment>();
            try
            {
                short accountId = approverAccountId ?? _accountRepository.GetAccountIdByAccountType(AccountTypeEnum.SysAdmin);

                IEnumerable<Enrollment> approvedEnrollments = _enrollmentRepository.GetAllByTrainingIdAndApprovalStatus(
                    training.TrainingId,
                    new List<ApprovalStatusEnum>() { ApprovalStatusEnum.Approved });
                if (!approvedEnrollments.Any())
                {
                    response.AddError(new ErrorModel() { Message = $"No enrollments for {training.TrainingName} yet." });
                    return response;
                }

                Employee approver = new Employee() { AccountId = accountId };
                if (training.SeatsAvailable >= approvedEnrollments.Count())
                {
                    approver.ConfirmEnrollments(approvedEnrollments);
                }
                else
                {
                    IEnumerable<Employee> approvedEmployees = _employeeRepository.GetAllByEmployeeIds(approvedEnrollments.Select(enrollment => enrollment.EmployeeId));
                    IEnumerable<short> confirmedEmployeesBasedOnTrainingPriority = approvedEmployees
                        .Where(employee => employee.DepartmentId == training.PreferredDepartmentId)
                        .Select(employee => employee.EmployeeId);

                    int enrollmentsToConfirmCount = Math.Min(training.SeatsAvailable, confirmedEmployeesBasedOnTrainingPriority.Count());
                    IEnumerable<Enrollment> enrollmentsToConfirm = approvedEnrollments
                        .Where(enrollment => confirmedEmployeesBasedOnTrainingPriority.Contains(enrollment.EmployeeId))
                        .OrderBy(enrollment => enrollment.RequestedAt)
                        .Take(enrollmentsToConfirmCount)
                        .Concat(approvedEnrollments
                                    .Where(enrollment => !confirmedEmployeesBasedOnTrainingPriority.Contains(enrollment.EmployeeId))
                                    .OrderBy(enrollment => enrollment.RequestedAt)
                                    .Take(training.SeatsAvailable - confirmedEmployeesBasedOnTrainingPriority.Count())
                        );
                    approver.ConfirmEnrollments(enrollmentsToConfirm);
                    approver.DeclineEnrollment(approvedEnrollments.Except(enrollmentsToConfirm));
                }
                // Assume both enrollment and notification inserted
                _enrollmentRepository.UpdateBatch(approvedEnrollments);
                _enrollmentNotificationRepository.AddBatch(approvedEnrollments.Select(enrollment =>
                    new EnrollmentNotification(
                        $"You have been {enrollment.ApprovalStatus} for training: {training.TrainingName}",
                        enrollment.EmployeeId)
                    ));
            }
            catch (DALException dalEx)
            {
                _logger.LogError(dalEx, $"Error in validating approved enrollment applications for training with id {training.TrainingId}");
                response.AddError(new ErrorModel()
                {
                    Message = $"Unable to validate approved enrollment applications for training: {training.TrainingName}.",
                    Exception = dalEx
                });
            }
            return response;
        }
    }
}
