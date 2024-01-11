using Core.Application.Models;
using Core.Application.Repositories;
using Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task<ResponseModel<EnrollmentViewModel>> ApproveAsync(int enrollmentId, short approverAccountId)
        {
            ResponseModel<EnrollmentViewModel> response = new ResponseModel<EnrollmentViewModel>();
            try
            {
                Enrollment enrollment = await _enrollmentRepository.GetAsync(enrollmentId);
                Employee approver = new Employee() { AccountId = approverAccountId };
                approver.ApproveEnrollment(enrollment);
                response.UpdatedRows = await _enrollmentRepository.Update(enrollment);
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

        public async Task<ResponseModel<EnrollmentViewModel>> DeclineAsync(DeclineEnrollmentViewModel declineEnrollmentViewModel, short approverAccountId)
        {
            ResponseModel<EnrollmentViewModel> response = new ResponseModel<EnrollmentViewModel>();
            try
            {
                Enrollment enrollment = await _enrollmentRepository.GetAsync(declineEnrollmentViewModel.EnrollmentId);
                Employee approver = new Employee() { AccountId = approverAccountId };
                approver.DeclineEnrollment(enrollment);
                response.UpdatedRows = await _enrollmentRepository.Update(enrollment);
                // Assume both enrollment and notification inserted
                Training training = await _trainingRepository.GetAsync(enrollment.TrainingId);
                await _enrollmentNotificationRepository.Add(new EnrollmentNotification(
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

        public async Task<ResponseModel<EnrollmentViewModel>> GetEnrollmentSubmissionsForApprovalAsync(short managerId)
        {
            ResponseModel<EnrollmentViewModel> response = new ResponseModel<EnrollmentViewModel>();
            try
            {
                IEnumerable<Enrollment> enrollments = await _enrollmentRepository.GetAllByManagerIdAndApprovalStatusAsync(
                    managerId,
                    new List<ApprovalStatusEnum>() {
                        ApprovalStatusEnum.Pending
                    });

                List<EnrollmentViewModel> enrollmentViewModels = new List<EnrollmentViewModel>();
                foreach (Enrollment enrollment in enrollments)
                {
                    Employee employee = await _employeeRepository.GetAsync(enrollment.EmployeeId);
                    Training training = await _trainingRepository.GetAsync(enrollment.TrainingId);
                    enrollmentViewModels.Add(new EnrollmentViewModel(enrollment)
                    {
                        EmployeeName = employee?.GetFullName(),
                        TrainingName = training?.TrainingName
                    });
                }
                response.Entities = enrollmentViewModels;
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

        public async Task<ResponseModel<EnrollmentViewModel>> GetEnrollmentsAsync(short employeeId)
        {
            ResponseModel<EnrollmentViewModel> response = new ResponseModel<EnrollmentViewModel>();
            try
            {
                IEnumerable<Enrollment> enrollments = await _enrollmentRepository.GetAllByEmployeeIdAndApprovalStatusAsync(
                    employeeId,
                    new List<ApprovalStatusEnum>() {
                        ApprovalStatusEnum.Pending, ApprovalStatusEnum.Approved, ApprovalStatusEnum.Declined, ApprovalStatusEnum.Confirmed});

                List<EnrollmentViewModel> enrollmentViewModels = new List<EnrollmentViewModel>();
                foreach (Enrollment enrollment in enrollments)
                {
                    Training training = await _trainingRepository.GetAsync(enrollment.TrainingId);
                    enrollmentViewModels.Add(new EnrollmentViewModel(enrollment)
                    {
                        TrainingName = training?.TrainingName
                    });
                }
                response.Entities = enrollmentViewModels;
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

        public async Task<ResponseModel<Enrollment>> SubmitAsync(short employeeId, short trainingId)
        {
            ResponseModel<Enrollment> response = new ResponseModel<Enrollment>();
            try
            {
                if (await _enrollmentRepository.Exists(employeeId, trainingId))
                {
                    response.AddError(new ErrorModel()
                    {
                        Message = $"User already has a pending enrollment submission."
                    });
                    return response;
                }
                response.AddedRows = await _enrollmentRepository.Add(new Enrollment()
                {
                    ApprovalStatus = ApprovalStatusEnum.Pending,
                    EmployeeId = employeeId,
                    TrainingId = trainingId
                });
                // Assume both enrollment and notification inserted
                Training training = await _trainingRepository.GetAsync(trainingId);
                Employee employee = await _employeeRepository.GetAsync(employeeId);
                await _enrollmentNotificationRepository.Add(new EnrollmentNotification(
                    $"{employee.GetFullName()} has submitted an enrollment application for training: {training.TrainingName}",
                    employee.ManagerId)
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

        public async Task<ResponseModel<Enrollment>> SubmitAsync(short employeeId, short trainingId, IEnumerable<EmployeeUpload> employeeUploads)
        {
            ResponseModel<Enrollment> response = new ResponseModel<Enrollment>();
            try
            {
                if (await _enrollmentRepository.Exists(employeeId, trainingId))
                {
                    response.AddError(new ErrorModel()
                    {
                        Message = $"User already has a pending enrollment submission."
                    });
                    return response;
                }
                response.AddedRows = await _enrollmentRepository.AddWithEmployeeUploads(new Enrollment()
                {
                    ApprovalStatus = ApprovalStatusEnum.Pending,
                    EmployeeId = employeeId,
                    TrainingId = trainingId
                }, employeeUploads);
                // Assume both enrollment and notification inserted
                Training training = await _trainingRepository.GetAsync(trainingId);
                Employee employee = await _employeeRepository.GetAsync(employeeId);
                await _enrollmentNotificationRepository.Add(new EnrollmentNotification(
                    $"{employee.GetFullName()} has submitted an enrollment application for training: {training.TrainingName}",
                    employee.ManagerId)
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

        public async Task<ResponseModel<ResponseModel<Enrollment>>> ValidateApprovedEnrollmentsAsync(short? approverAccountId)
        {
            ResponseModel<ResponseModel<Enrollment>> response = new ResponseModel<ResponseModel<Enrollment>>();
            try
            {
                short accountId = approverAccountId ?? await _accountRepository.GetAccountIdByAccountType(AccountTypeEnum.SysAdmin);

                IEnumerable<Training> trainings = await _trainingRepository.GetAllByRegistrationDeadlineDueAsync(DateTime.Now);
                List<ResponseModel<Enrollment>> trainingEnrollmentsResponses = new List<ResponseModel<Enrollment>>();
                foreach (Training training in trainings)
                {
                    ResponseModel<Enrollment> responseModel = await ValidateApprovedEnrollmentsByTrainingAsync(accountId, training);
                    trainingEnrollmentsResponses.Add(responseModel);
                }
                response.Entities = trainingEnrollmentsResponses;
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

        public async Task<ResponseModel<Enrollment>> ValidateApprovedEnrollmentsByTrainingAsync(short approverAccountId, short trainingId)
        {
            ResponseModel<Enrollment> response = new ResponseModel<Enrollment>();
            try
            {
                Training training = await _trainingRepository.GetAsync(trainingId);
                response = await ValidateApprovedEnrollmentsByTrainingAsync(approverAccountId, training);
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

        private async Task<ResponseModel<Enrollment>> ValidateApprovedEnrollmentsByTrainingAsync(short? approverAccountId, Training training)
        {
            ResponseModel<Enrollment> response = new ResponseModel<Enrollment>();
            try
            {
                short accountId = approverAccountId ?? await _accountRepository.GetAccountIdByAccountType(AccountTypeEnum.SysAdmin);

                IEnumerable<Enrollment> approvedEnrollments = await _enrollmentRepository.GetAllByTrainingIdAndApprovalStatusAsync(
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
                    IEnumerable<Employee> approvedEmployees = await _employeeRepository.GetAllByEmployeeIdsAsync(approvedEnrollments.Select(enrollment => enrollment.EmployeeId));
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
                await _enrollmentRepository.UpdateBatch(approvedEnrollments);
                await _enrollmentNotificationRepository.AddBatch(approvedEnrollments.Select(enrollment =>
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
