using Core.Application;
using Core.Application.Models;
using Core.Application.Repositories;
using Core.Application.Services;
using Core.Domain;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Tests.Services
{
    [TestFixture]
    public class EnrollmentServiceTest
    {
        private Mock<IAccountRepository> _stubAccountRepository;
        private Mock<IEmployeeRepository> _stubEmployeeRepository;
        private Mock<IEnrollmentRepository> _stubEnrollmentRepository;
        private Mock<ILogger> _stubLogger;
        private Mock<ITrainingRepository> _stubTrainingRepository;
        private Mock<IUserNotificationRepository> _stubUserNotificationRepository;

        private IEnrollmentService _enrollmentService;

        [SetUp]
        public void Setup()
        {
            List<Account> accounts = new List<Account>()
            {
                new Account()
                {
                    AccountId = 1,
                    AccountType = AccountTypeEnum.SysAdmin
                }
            };

            List<Employee> employees = new List<Employee>()
            {
                new Employee()
                {
                    EmployeeId = 1,
                    DepartmentId = 1
                },
                new Employee()
                {
                    EmployeeId = 2,
                    DepartmentId = 1
                },
                new Employee()
                {
                    EmployeeId = 3,
                    DepartmentId = 1
                },
                new Employee()
                {
                    EmployeeId = 4,
                    DepartmentId = 2
                }
            };

            List<Training> trainings = new List<Training>()
            {
                new Training() {
                    TrainingId = 1,
                    PreferredDepartmentId = 1,
                    RegistrationDeadline = DateTime.Now.AddDays(-1),
                    SeatsAvailable = 2,
                    TrainingName = "Training 1"
                },
                new Training() {
                    TrainingId = 2,
                    PreferredDepartmentId = 1,
                    RegistrationDeadline = DateTime.Now.AddDays(-1),
                    SeatsAvailable = 2,
                    TrainingName = "Training 2"
                },
                new Training() {
                    TrainingId = 3,
                    PreferredDepartmentId = 2,
                    RegistrationDeadline = DateTime.Now.AddDays(-1),
                    SeatsAvailable = 2,
                    TrainingName = "Training 2"
                }
            };

            List<Enrollment> enrollments = new List<Enrollment>()
            {
                new Enrollment()
                {
                    ApprovalStatus = ApprovalStatusEnum.Approved,
                    EmployeeId = 1,
                    EnrollmentId = 1,
                    RequestedAt = DateTime.Now.AddDays(-2),
                    TrainingId = 1
                },
                new Enrollment()
                {
                    ApprovalStatus = ApprovalStatusEnum.Approved,
                    EmployeeId = 1,
                    EnrollmentId = 2,
                    RequestedAt = DateTime.Now.AddDays(-4),
                    TrainingId = 2
                },
                new Enrollment()
                {
                    ApprovalStatus = ApprovalStatusEnum.Approved,
                    EmployeeId = 2,
                    EnrollmentId = 3,
                    RequestedAt = DateTime.Now.AddDays(-3),
                    TrainingId = 2
                },
                new Enrollment()
                {
                    ApprovalStatus = ApprovalStatusEnum.Approved,
                    EmployeeId = 3,
                    EnrollmentId = 4,
                    RequestedAt = DateTime.Now.AddDays(-2),
                    TrainingId = 2
                },
                new Enrollment()
                {
                    ApprovalStatus = ApprovalStatusEnum.Approved,
                    EmployeeId = 1,
                    EnrollmentId = 5,
                    RequestedAt = DateTime.Now.AddDays(-4),
                    TrainingId = 3
                },
                new Enrollment()
                {
                    ApprovalStatus = ApprovalStatusEnum.Approved,
                    EmployeeId = 2,
                    EnrollmentId = 6,
                    RequestedAt = DateTime.Now.AddDays(-3),
                    TrainingId = 3
                },
                new Enrollment()
                {
                    ApprovalStatus = ApprovalStatusEnum.Approved,
                    EmployeeId = 4,
                    EnrollmentId = 7,
                    RequestedAt = DateTime.Now.AddDays(-2),
                    TrainingId = 3
                }
            };

            _stubAccountRepository = new Mock<IAccountRepository>();
            _stubEmployeeRepository = new Mock<IEmployeeRepository>();
            _stubEnrollmentRepository = new Mock<IEnrollmentRepository>();
            _stubLogger = new Mock<ILogger>();
            _stubTrainingRepository = new Mock<ITrainingRepository>();
            _stubUserNotificationRepository = new Mock<IUserNotificationRepository>();

            _stubAccountRepository
                .Setup(iAccountRepository => iAccountRepository.GetAccountIdByAccountType(It.IsAny<AccountTypeEnum>()))
                .ReturnsAsync((AccountTypeEnum accountType) =>
                    accounts.Where(account => account.AccountType == accountType).First().AccountId
                 );

            _stubEmployeeRepository
                .Setup(iEmployeeRepository => iEmployeeRepository.GetAllByEmployeeIdsAsync(It.IsAny<IEnumerable<short>>()))
                .ReturnsAsync((IEnumerable<short> employeeIds) =>
                    employees.Where(employee => employeeIds.Any(employeeId => employeeId == employee.EmployeeId)).ToList()
                );

            _stubEnrollmentRepository
                .Setup(iEnrollmentRepository => iEnrollmentRepository.GetAllByTrainingIdAndApprovalStatusAsync(It.IsAny<short>(), It.IsAny<IEnumerable<ApprovalStatusEnum>>()))
                .ReturnsAsync((short trainingId, IEnumerable<ApprovalStatusEnum> approvalStatuses) =>
                    enrollments
                        .Where(enrollment =>
                            enrollment.TrainingId == trainingId &&
                            approvalStatuses.Any(approvalStatus => approvalStatus == enrollment.ApprovalStatus))
                        .ToList()
                );

            _stubEnrollmentRepository
                .Setup(iEnrollmentRepository => iEnrollmentRepository.UpdateBatch(It.IsAny<IEnumerable<Enrollment>>()))
                .ReturnsAsync((IEnumerable<Enrollment> updatedEnrollments) => default);

            _stubLogger
                .Setup(iLogger => iLogger.LogError(It.IsAny<Exception>(), It.IsAny<string>()))
                .Callback((Exception ex, string message) => { });

            _stubTrainingRepository
                .Setup(iTrainingRepository => iTrainingRepository.GetAsync(It.IsAny<short>()))
                .ReturnsAsync((short trainingId) => trainings.Where(training => training.TrainingId == trainingId).First());

            _stubUserNotificationRepository
                .Setup(iUserNotificationRepository => iUserNotificationRepository.AddBatch(It.IsAny<IEnumerable<UserNotification>>()))
                .ReturnsAsync((IEnumerable<UserNotification> notifications) => default);

            _enrollmentService = new EnrollmentService(
                _stubEnrollmentRepository.Object,
                _stubEmployeeRepository.Object,
                _stubLogger.Object,
                _stubTrainingRepository.Object,
                _stubAccountRepository.Object,
                _stubUserNotificationRepository.Object);
        }

        [TestCase(1, 1)]
        public async Task ValidateApprovedEnrollmentsByTraining_ApproverAccountIdAndTrainingId_ReturnsSuccessAsync(short approverAccountId, short trainingId)
        {
            Result actualResult = await _enrollmentService.ValidateApprovedEnrollmentsByTrainingAsync(approverAccountId, trainingId);

            Assert.IsTrue(actualResult.IsSuccess);
        }

        [TestCaseSource(nameof(SeatsAvailableGreaterThanOrEqualToNumberOfApprovedEnrollmentsTestCase))]
        public async Task ValidateApprovedEnrollmentsByTraining_SeatsAvailableGreaterThanOrEqualToNumberOfApprovedEnrollments_AllApprovedEnrollmentsConfirmedAsync((short, short, IEnumerable<Enrollment>) testData)
        {
            await ValidateApprovedEnrollmentsByTraining_SeatsAvailableAndNumberOfApprovedEnrollments_ApprovedEnrollmentsSortedAndConfirmedByTrainingPriorityAsync(testData);
        }

        [TestCaseSource(nameof(SeatsAvailableLessThanNumberOfApprovedEnrollmentsTestCases))]
        public async Task ValidateApprovedEnrollmentsByTraining_SeatsAvailableLessThanNumberOfApprovedEnrollments_ApprovedEnrollmentsSortedAndConfirmedByTrainingPriorityAsync((short, short, IEnumerable<Enrollment>) testData)
        {
            await ValidateApprovedEnrollmentsByTraining_SeatsAvailableAndNumberOfApprovedEnrollments_ApprovedEnrollmentsSortedAndConfirmedByTrainingPriorityAsync(testData);
        }

        private async Task ValidateApprovedEnrollmentsByTraining_SeatsAvailableAndNumberOfApprovedEnrollments_ApprovedEnrollmentsSortedAndConfirmedByTrainingPriorityAsync((short ApproverAccountId, short TrainingId, IEnumerable<Enrollment> ExpectedUpdatedEnrollments) testData)
        {
            await _enrollmentService.ValidateApprovedEnrollmentsByTrainingAsync(testData.ApproverAccountId, testData.TrainingId);

            IEnumerable<Enrollment> actualUpdatedEnrollments = await _stubEnrollmentRepository.Object.GetAllByTrainingIdAndApprovalStatusAsync(
                testData.TrainingId,
                new List<ApprovalStatusEnum>() { ApprovalStatusEnum.Confirmed, ApprovalStatusEnum.Declined });

            CollectionAssert.AreEquivalent(testData.ExpectedUpdatedEnrollments, actualUpdatedEnrollments);
            CollectionAssert.AreEquivalent(
                testData.ExpectedUpdatedEnrollments.Where(enrollment => enrollment.ApprovalStatus == ApprovalStatusEnum.Confirmed),
                actualUpdatedEnrollments.Where(enrollment => enrollment.ApprovalStatus == ApprovalStatusEnum.Confirmed));
            CollectionAssert.AreEquivalent(
                testData.ExpectedUpdatedEnrollments.Where(enrollment => enrollment.ApprovalStatus == ApprovalStatusEnum.Declined),
                actualUpdatedEnrollments.Where(enrollment => enrollment.ApprovalStatus == ApprovalStatusEnum.Declined));
        }

        private static IEnumerable<(short, short, IEnumerable<Enrollment>)> SeatsAvailableGreaterThanOrEqualToNumberOfApprovedEnrollmentsTestCase()
        {
            yield return (1, 1, new List<Enrollment>()
            {
                new Enrollment()
                {
                    ApprovalStatus = ApprovalStatusEnum.Confirmed,
                    EmployeeId = 1,
                    EnrollmentId = 1,
                    RequestedAt = DateTime.Now.AddDays(-2),
                    TrainingId = 1
                }
            });
            // Add test data for SeatsAvailable == NumberOfApprovedEnrollments
        }

        private static IEnumerable<(short, short, IEnumerable<Enrollment>)> SeatsAvailableLessThanNumberOfApprovedEnrollmentsTestCases()
        {
            yield return (1, 2, new List<Enrollment>()
            {
                new Enrollment()
                {
                    ApprovalStatus = ApprovalStatusEnum.Confirmed,
                    EmployeeId = 1,
                    EnrollmentId = 2,
                    RequestedAt = DateTime.Now.AddDays(-4),
                    TrainingId = 2
                },
                new Enrollment()
                {
                    ApprovalStatus = ApprovalStatusEnum.Confirmed,
                    EmployeeId = 2,
                    EnrollmentId = 3,
                    RequestedAt = DateTime.Now.AddDays(-3),
                    TrainingId = 2
                },
                new Enrollment()
                {
                    ApprovalStatus = ApprovalStatusEnum.Declined,
                    EmployeeId = 3,
                    EnrollmentId = 4,
                    RequestedAt = DateTime.Now.AddDays(-2),
                    TrainingId = 2
                }
            });

            yield return (1, 3, new List<Enrollment>()
            {
                new Enrollment()
                {
                    ApprovalStatus = ApprovalStatusEnum.Confirmed,
                    EmployeeId = 1,
                    EnrollmentId = 5,
                    RequestedAt = DateTime.Now.AddDays(-4),
                    TrainingId = 3
                },
                new Enrollment()
                {
                    ApprovalStatus = ApprovalStatusEnum.Declined,
                    EmployeeId = 2,
                    EnrollmentId = 6,
                    RequestedAt = DateTime.Now.AddDays(-3),
                    TrainingId = 3
                },
                new Enrollment()
                {
                    ApprovalStatus = ApprovalStatusEnum.Confirmed,
                    EmployeeId = 4,
                    EnrollmentId = 7,
                    RequestedAt = DateTime.Now.AddDays(-2),
                    TrainingId = 3
                }
            });

        }
    }
}
