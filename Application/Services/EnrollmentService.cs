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
        private readonly IUserNotificationRepository _userNotificationRepository;
        private readonly ILogger _logger;
        private readonly ITrainingRepository _trainingRepository;

        public EnrollmentService(IEnrollmentRepository enrollmentRepository, IEmployeeRepository employeeRepository, ILogger logger, ITrainingRepository trainingRepository, IAccountRepository accountRepository, IUserNotificationRepository userNotificationRepository)
        {
            _enrollmentRepository = enrollmentRepository;
            _employeeRepository = employeeRepository;
            _logger = logger;
            _trainingRepository = trainingRepository;
            _accountRepository = accountRepository;
            _userNotificationRepository = userNotificationRepository;
        }

        public async Task<Result> ApproveAsync(int enrollmentId, short approverAccountId)
        {
            try
            {
                Enrollment enrollment = await _enrollmentRepository.GetAsync(enrollmentId);
                Employee approver = new Employee() { AccountId = approverAccountId };
                approver.ApproveEnrollment(enrollment);

                await _enrollmentRepository.Update(enrollment);

                return Result.Success();
            }
            catch (DALException dalEx)
            {
                _logger.LogError(dalEx, "Error in approving enrollment");
            }
            catch (MapperException mapperEx)
            {
                _logger.LogError(mapperEx, "Error in mapper");
            }

            return Result.Failure(new Error("Unable to process enrollment. Try again later."));
        }

        public async Task<Result> DeclineAsync(DeclineEnrollmentViewModel declineEnrollmentViewModel, short approverAccountId)
        {
            try
            {
                Enrollment enrollment = await _enrollmentRepository.GetAsync(declineEnrollmentViewModel.EnrollmentId);
                Employee approver = new Employee() { AccountId = approverAccountId };
                approver.DeclineEnrollment(enrollment);

                await _enrollmentRepository.Update(enrollment);

                // Assume both enrollment and notification inserted
                Training training = await _trainingRepository.GetAsync(enrollment.TrainingId);
                if (training is null)
                {
                    return Result.Failure(new Error("Could not send notification."));
                }

                await _userNotificationRepository.Add(new UserNotification(
                    "ENROLLMENT REQUEST DECLINED",
                    $"Your enrollment application for training: {training.TrainingName} has been declined. Reason: {declineEnrollmentViewModel.ReasonMessage}",
                    enrollment.EmployeeId));

                return Result.Success();
            }
            catch (DALException dalEx)
            {
                _logger.LogError(dalEx, "Error in declining enrollment");
            }
            catch (MapperException mapperEx)
            {
                _logger.LogError(mapperEx, "Error in mapper");
            }

            return Result.Failure(new Error("Unable to process enrollment. Try again later."));
        }

        public async Task<ResultT<IEnumerable<EnrollmentViewModel>>> GetEnrollmentSubmissionsForApprovalAsync(short managerId)
        {
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

                return ResultT<IEnumerable<EnrollmentViewModel>>.Success(enrollmentViewModels);
            }
            catch (DALException dalEx)
            {
                _logger.LogError(dalEx, "Error in retrieving enrollments");
            }
            catch (MapperException mapperEx)
            {
                _logger.LogError(mapperEx, "Error in mapper");
            }

            return ResultT<IEnumerable<EnrollmentViewModel>>.Failure(new Error("Unable to retrieve enrollments."));
        }

        public async Task<ResultT<IEnumerable<EnrollmentViewModel>>> GetEnrollmentsAsync(short employeeId)
        {
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

                return ResultT<IEnumerable<EnrollmentViewModel>>.Success(enrollmentViewModels);
            }
            catch (DALException dalEx)
            {
                _logger.LogError(dalEx, "Error in retrieving enrollments");
            }
            catch (MapperException mapperEx)
            {
                _logger.LogError(mapperEx, "Error in mapper");
            }

            return ResultT<IEnumerable<EnrollmentViewModel>>.Failure(new Error("Unable to retrieve enrollments."));
        }

        public async Task<Result> SubmitAsync(short employeeId, short trainingId)
        {
            try
            {
                if (await _enrollmentRepository.Exists(employeeId, trainingId))
                {
                    return Result.Failure(new Error($"User already has a pending enrollment submission."));
                }

                await _enrollmentRepository.Add(new Enrollment()
                {
                    ApprovalStatus = ApprovalStatusEnum.Pending,
                    EmployeeId = employeeId,
                    TrainingId = trainingId
                });

                // Assume both enrollment and notification inserted
                Training training = await _trainingRepository.GetAsync(trainingId);
                if (training is null)
                {
                    return Result.Failure(new Error("Could not send notification."));
                }

                Employee employee = await _employeeRepository.GetAsync(employeeId);
                if (employee is null)
                {
                    return Result.Failure(new Error("Could not send notification."));
                }

                await _userNotificationRepository.Add(new UserNotification(
                    "ENROLLMENT REQUEST",
                    $"{employee.GetFullName()} has submitted an enrollment application for training: {training.TrainingName}",
                    employee.ManagerId)
                );

                return Result.Success();
            }
            catch (DALException dalEx)
            {
                _logger.LogError(dalEx, "Error in submitting enrollment application");
            }
            catch (MapperException mapperEx)
            {
                _logger.LogError(mapperEx, "Error in mapper");
            }

            return Result.Failure(new Error("Training registration failed. Try again later."));
        }

        public async Task<Result> SubmitAsync(short employeeId, short trainingId, IEnumerable<EmployeeUpload> employeeUploads)
        {
            try
            {
                if (await _enrollmentRepository.Exists(employeeId, trainingId))
                {
                    return Result.Failure(new Error($"User already has a pending enrollment submission."));
                }

                await _enrollmentRepository.AddWithEmployeeUploads(new Enrollment()
                {
                    ApprovalStatus = ApprovalStatusEnum.Pending,
                    EmployeeId = employeeId,
                    TrainingId = trainingId
                }, employeeUploads);

                // Assume both enrollment and notification inserted
                Training training = await _trainingRepository.GetAsync(trainingId);
                if (training is null)
                {
                    return Result.Failure(new Error("Could not send notification."));
                }

                Employee employee = await _employeeRepository.GetAsync(employeeId);
                if (employee is null)
                {
                    return Result.Failure(new Error("Could not send notification."));
                }

                await _userNotificationRepository.Add(new UserNotification(
                    $"ENROLLMENT REQUEST",
                    $"{employee.GetFullName()} has submitted an enrollment application for training: {training.TrainingName}",
                    employee.ManagerId)
                );

                return Result.Success();
            }
            catch (DALException dalEx)
            {
                _logger.LogError(dalEx, "Error in submitting enrollment application");
            }
            catch (MapperException mapperEx)
            {
                _logger.LogError(mapperEx, "Error in mapper");
            }

            return ResultT<Result>.Failure(new Error("Training registration failed. Try again later."));
        }

        public async Task<ResultT<IEnumerable<Result>>> ValidateApprovedEnrollmentsAsync(short? approverAccountId)
        {
            try
            {
                short accountId = approverAccountId ?? await _accountRepository.GetAccountIdByAccountType(AccountTypeEnum.SysAdmin);

                IEnumerable<Training> trainings = await _trainingRepository.GetAllByRegistrationDeadlineDueAsync(DateTime.Now);
                if (trainings.Count() == 0)
                {
                    return ResultT<IEnumerable<Result>>.Failure(new Error("No trainings with registration deadline due found."));
                }

                List<Result> trainingEnrollmentsResults = new List<Result>();
                foreach (Training training in trainings)
                {
                    trainingEnrollmentsResults.Add(await ValidateApprovedEnrollmentsByTrainingAsync(accountId, training));
                }

                return ResultT<IEnumerable<Result>>.Success(trainingEnrollmentsResults);
            }
            catch (DALException dalEx)
            {
                _logger.LogError(dalEx, "Error in retrieving trainings with due registration deadline");
            }
            catch (MapperException mapperEx)
            {
                _logger.LogError(mapperEx, "Error in mapper");
            }

            return ResultT<IEnumerable<Result>>.Failure(new Error("Unable to validate approved enrollment applications. Try again later."));
        }

        public async Task<Result> ValidateApprovedEnrollmentsByTrainingAsync(short approverAccountId, short trainingId)
        {
            try
            {
                Training training = await _trainingRepository.GetAsync(trainingId);
                if (training is null)
                {
                    return Result.Failure(new Error("Could not retrieve training."));
                }

                if (training.RegistrationDeadline >= DateTime.Now)
                {
                    return Result.Failure(new Error("Registration deadline for training is not due yet."));
                }

                return await ValidateApprovedEnrollmentsByTrainingAsync(approverAccountId, training);
            }
            catch (DALException dalEx)
            {
                _logger.LogError(dalEx, $"Error in retrieving training with id {trainingId}");
            }
            catch (MapperException mapperEx)
            {
                _logger.LogError(mapperEx, "Error in mapper");
            }

            return Result.Failure(new Error("UUnable to validate approved enrollment applications for training. Try again later."));
        }

        private async Task<Result> ValidateApprovedEnrollmentsByTrainingAsync(short? approverAccountId, Training training)
        {
            try
            {
                short accountId = approverAccountId ?? await _accountRepository.GetAccountIdByAccountType(AccountTypeEnum.SysAdmin);

                IEnumerable<Enrollment> approvedEnrollments = await _enrollmentRepository.GetAllByTrainingIdAndApprovalStatusAsync(
                    training.TrainingId,
                    new List<ApprovalStatusEnum>() { ApprovalStatusEnum.Approved });
                if (approvedEnrollments.Count() == 0)
                {
                    return Result.Failure(new Error($"No approved enrollments for {training.TrainingName}."));
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
                await _userNotificationRepository.AddBatch(approvedEnrollments.Select(enrollment =>
                    new UserNotification(
                        $"ENROLLMENT REQUEST {enrollment.ApprovalStatus}",
                        $"You have been {enrollment.ApprovalStatus} for training: {training.TrainingName}",
                        enrollment.EmployeeId)
                    ));

                return Result.Success();
            }
            catch (DALException dalEx)
            {
                _logger.LogError(dalEx, $"Error in validating approved enrollment applications for training with id {training.TrainingId}");
            }
            catch (MapperException mapperEx)
            {
                _logger.LogError(mapperEx, "Error in mapper");
            }

            return Result.Failure(new Error($"Unable to validate approved enrollment applications for training: {training.TrainingName}."));
        }
    }
}
