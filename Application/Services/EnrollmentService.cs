using Core.Application.Models;
using Core.Application.Repositories;
using Core.Domain;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Core.Application.Services
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IEnrollmentRepository _enrollmentRepository;
        private readonly IUserNotificationRepository _userNotificationRepository;
        private readonly ITrainingRepository _trainingRepository;

        private readonly ILogger _logger;
        private readonly IReportGenerationService _reportGenerationService;

        public EnrollmentService(IEnrollmentRepository enrollmentRepository, IEmployeeRepository employeeRepository, ILogger logger, ITrainingRepository trainingRepository, IAccountRepository accountRepository, IUserNotificationRepository userNotificationRepository, IReportGenerationService reportGenerationService)
        {
            _enrollmentRepository = enrollmentRepository;
            _employeeRepository = employeeRepository;
            _logger = logger;
            _trainingRepository = trainingRepository;
            _accountRepository = accountRepository;
            _userNotificationRepository = userNotificationRepository;
            _reportGenerationService = reportGenerationService;
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

        public async Task<ResultT<Stream>> GenerateEnrollmentReportAsync()
        {
            try
            {
                IEnumerable<Training> trainings = await _trainingRepository.GetAllByRegistrationDeadlineDueAsync(DateTime.Now);
                if (trainings.Count() == 0)
                {
                    return ResultT<Stream>.Failure(new Error("No trainings with registration deadline due found."));
                }

                Stream outputStream = new MemoryStream();
                foreach (Training training in trainings)
                {
                    await GenerateEnrollmentReportByTrainingAsync(training, outputStream);
                }

                outputStream.Position = 0;
                return ResultT<Stream>.Success(outputStream);
            }
            catch (DALException dalEx)
            {
                _logger.LogError(dalEx, $"Error in generating enrollment report.");
            }
            catch (MapperException mapperEx)
            {
                _logger.LogError(mapperEx, "Error in mapper");
            }

            return ResultT<Stream>.Failure(new Error("Unable to generate enrollment report. Try again later."));
        }

        public async Task<ResultT<Stream>> GenerateEnrollmentReportByTrainingAsync(short trainingId)
        {
            try
            {
                Training training = await _trainingRepository.GetAsync(trainingId);
                if (training is null)
                {
                    return ResultT<Stream>.Failure(new Error("Could not retrieve training."));
                }

                if (training.RegistrationDeadline >= DateTime.Now)
                {
                    return ResultT<Stream>.Failure(new Error("Registration deadline for training is not due yet."));
                }

                ResultT<Stream> result = await GenerateEnrollmentReportByTrainingAsync(training, new MemoryStream());
                if (result.IsSuccess)
                {
                    result.Value.Position = 0;
                }
                return result;
            }
            catch (DALException dalEx)
            {
                _logger.LogError(dalEx, $"Error in retrieving training with id {trainingId}");
            }
            catch (MapperException mapperEx)
            {
                _logger.LogError(mapperEx, "Error in mapper");
            }

            return ResultT<Stream>.Failure(new Error("Unable to generate enrollment report for training. Try again later."));
        }

        private async Task<ResultT<Stream>> GenerateEnrollmentReportByTrainingAsync(Training training, Stream outputStream)
        {
            try
            {
                IEnumerable<Enrollment> confirmedEnrollments = await _enrollmentRepository.GetAllByTrainingIdAndApprovalStatusAsync(
                    training.TrainingId,
                    new List<ApprovalStatusEnum>() { ApprovalStatusEnum.Confirmed });
                if (confirmedEnrollments.Count() == 0)
                {
                    return ResultT<Stream>.Failure(new Error($"No confirmed enrollments for {training.TrainingName}."));
                }

                IEnumerable<Employee> confirmedEmployees = await _employeeRepository.GetAllByEmployeeIdsAsync(confirmedEnrollments.Select(enrollment => enrollment.EmployeeId));
                IEnumerable<Employee> managers = await _employeeRepository.GetAllByEmployeeIdsAsync(confirmedEmployees.Select(employee => employee.ManagerId).Distinct());

                IEnumerable<object[]> reportRows = confirmedEmployees.Join(managers,
                                                         confirmedEmployee => confirmedEmployee.ManagerId,
                                                         manager => manager.EmployeeId,
                                                         (confirmedEmployee, manager) =>
                                                            new object[]
                                                            {
                                                                confirmedEmployee.GetFullName(),
                                                                confirmedEmployee.MobileNumber,
                                                                manager.GetFullName()
                                                            });
                string[] rowHeaders = new string[] { "Employee FullName", "Employee Mobile Number", "Manager FullName" };

                await _reportGenerationService.GenerateReportAsync(rowHeaders, reportRows, training.TrainingName, outputStream);

                return ResultT<Stream>.Success(outputStream);
            }
            catch (DALException dalEx)
            {
                _logger.LogError(dalEx, $"Error in generating enrollment report for training with id {training.TrainingId}");
            }
            catch (MapperException mapperEx)
            {
                _logger.LogError(mapperEx, "Error in mapper");
            }

            return ResultT<Stream>.Failure(new Error("Unable to generate enrollment report for training. Try again later."));
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

        public async Task<Result> SubmitAsync(short employeeId, EnrollmentSubmissionViewModel enrollmentSubmissionViewModel)
        {
            try
            {
                if (await _enrollmentRepository.Exists(employeeId, enrollmentSubmissionViewModel.TrainingId))
                {
                    return Result.Failure(new Error($"User already has a current enrollment submission."));
                }

                Training training = await _trainingRepository.GetAsync(enrollmentSubmissionViewModel.TrainingId);
                if (training is null)
                {
                    return Result.Failure(new Error("Could not send notification."));
                }

                if (training.RegistrationDeadline <= DateTime.Now)
                {
                    return Result.Failure(new Error("Registration deadline for training is due."));
                }

                if (enrollmentSubmissionViewModel.EmployeeUploads is null)
                {
                    await _enrollmentRepository.Add(new Enrollment()
                    {
                        ApprovalStatus = ApprovalStatusEnum.Pending,
                        EmployeeId = employeeId,
                        TrainingId = enrollmentSubmissionViewModel.TrainingId
                    });
                }
                else
                {
                    // TODO: Move validation logic to Domain layer
                    // TODO: Return which file was invalid
                    if (!IsValidFiles(enrollmentSubmissionViewModel.EmployeeUploads))
                    {
                        return Result.Failure(new Error("Invalid files uploaded."));
                    }
                    // TODO: Handle saving file exceptions
                    IEnumerable<EmployeeUpload> employeeUploads = await SaveUploadedFilesAsync(enrollmentSubmissionViewModel.EmployeeUploads, enrollmentSubmissionViewModel.PrerequisiteIds);
                    await _enrollmentRepository.AddWithEmployeeUploads(new Enrollment()
                    {
                        ApprovalStatus = ApprovalStatusEnum.Pending,
                        EmployeeId = employeeId,
                        TrainingId = enrollmentSubmissionViewModel.TrainingId
                    }, employeeUploads);
                }

                // Assume both enrollment and notification inserted
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
            catch (ConfigurationErrorsException configEx)
            {
                _logger.LogError(configEx, "Error in configuration");
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

        public async Task<ResultT<IEnumerable<(string, Result)>>> ValidateApprovedEnrollmentsAsync(short? approverAccountId)
        {
            try
            {
                short accountId = approverAccountId ?? await _accountRepository.GetAccountIdByAccountType(AccountTypeEnum.SysAdmin);

                IEnumerable<Training> trainings = await _trainingRepository.GetAllByRegistrationDeadlineDueAsync(DateTime.Now);
                if (trainings.Count() == 0)
                {
                    return ResultT<IEnumerable<(string, Result)>>.Failure(new Error("No trainings with registration deadline due found."));
                }

                List<(string, Result)> trainingEnrollmentsResults = new List<(string, Result)>();
                foreach (Training training in trainings)
                {
                    trainingEnrollmentsResults.Add((training.TrainingName, await ValidateApprovedEnrollmentsByTrainingAsync(accountId, training)));
                }

                return ResultT<IEnumerable<(string, Result)>>.Success(trainingEnrollmentsResults);
            }
            catch (DALException dalEx)
            {
                _logger.LogError(dalEx, "Error in validating approved enrollment applications.");
            }
            catch (MapperException mapperEx)
            {
                _logger.LogError(mapperEx, "Error in mapper");
            }

            return ResultT<IEnumerable<(string, Result)>>.Failure(new Error("Unable to validate approved enrollment applications. Try again later."));
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

            return Result.Failure(new Error("Unable to validate approved enrollment applications for training. Try again later."));
        }

        private string GenerateUniqueFileName(string originalFileName)
        {
            string uniqueIdentifier = Guid.NewGuid().ToString("N");
            string fileExtension = Path.GetExtension(originalFileName);
            return $"{uniqueIdentifier}_{DateTime.Now:yyyyMMddHHmmss}{fileExtension}";
        }

        private bool IsValidFiles(IEnumerable<HttpPostedFileBase> files)
        {
            return files.All(file => IsValidFile(file));
        }

        private bool IsValidFile(HttpPostedFileBase file)
        {
            return file != null && IsValidFileSize(file) && IsValidFileType(file);
        }

        private bool IsValidFileSize(HttpPostedFileBase file)
        {
            return file.ContentLength > 0 && file.ContentLength <= (1024 * 1024);
        }

        private bool IsValidFileType(HttpPostedFileBase file)
        {
            List<string> allowedMimeTypes = new List<string> { "image/png", "application/pdf" };

            return allowedMimeTypes.Contains(file.ContentType);
        }

        private async Task<IEnumerable<EmployeeUpload>> SaveUploadedFilesAsync(IEnumerable<HttpPostedFileBase> uploadedFiles, IEnumerable<byte> prerequisiteIds)
        {
            string uploadsFolder = ConfigurationManager.AppSettings["UploadFolderPath"] ?? throw new ConfigurationErrorsException("Invalid or missing configuration for 'UploadFolderPath'.");

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            IEnumerable<Task<EmployeeUpload>> saveUploadedFileTasks = uploadedFiles
                .Zip(prerequisiteIds, (file, prerequisiteId) => new { File = file, PrerequisiteId = prerequisiteId })
                .Select(data => SaveUploadedFileAsync(data.File, data.PrerequisiteId, uploadsFolder));

            return await Task.WhenAll(saveUploadedFileTasks);
        }


        private async Task<EmployeeUpload> SaveUploadedFileAsync(HttpPostedFileBase uploadedFile, byte prerequisiteId, string uploadsFolder)
        {
            string fileName = Path.GetFileName(uploadedFile.FileName);
            string uniqueFileName = GenerateUniqueFileName(fileName);
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, bufferSize: 4096, useAsync: true))
            {
                await uploadedFile.InputStream.CopyToAsync(fileStream);
            }

            return new EmployeeUpload() { PrerequisiteId = prerequisiteId, UploadedFileName = uniqueFileName };
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
