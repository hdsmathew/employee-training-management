using Core.Application;
using Core.Application.Repositories;
using Core.Application.Services;
using Infrastructure.AppLogger;
using Infrastructure.DAL;
using Infrastructure.DAL.Interfaces;
using Infrastructure.Models;
using Infrastructure.ReportGeneration;
using Infrastructure.Repositories;
using System;
using Unity;

namespace WebMVC
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public static class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container =
          new Lazy<IUnityContainer>(() =>
          {
              var container = new UnityContainer();
              RegisterTypes(container);
              return container;
          });

        /// <summary>
        /// Configured Unity Container.
        /// </summary>
        public static IUnityContainer Container => container.Value;
        #endregion

        /// <summary>
        /// Registers the type mappings with the Unity container.
        /// </summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>
        /// There is no need to register concrete types such as controllers or
        /// API controllers (unless you want to change the defaults), as Unity
        /// allows resolving a concrete type even if it was not previously
        /// registered.
        /// </remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            // NOTE: To load from web.config uncomment the line below.
            // Make sure to add a Unity.Configuration to the using statements.
            // container.LoadConfiguration();

            // TODO: Register your type's mappings here.
            // container.RegisterType<IProductRepository, ProductRepository>();

            container.RegisterType<DataAccess>(TypeLifetime.Singleton);

            container.RegisterType<ILogger, FileLogger>(TypeLifetime.Singleton);

            container.RegisterType<AccountMapper>(TypeLifetime.Singleton);
            container.RegisterType<DepartmentMapper>(TypeLifetime.Singleton);
            container.RegisterType<EmployeeMapper>(TypeLifetime.Singleton);
            container.RegisterType<EmployeeUploadMapper>(TypeLifetime.Singleton);
            container.RegisterType<EnrollmentMapper>(TypeLifetime.Singleton);
            container.RegisterType<UserNotificationMapper>(TypeLifetime.Singleton);
            container.RegisterType<PrerequisiteMapper>(TypeLifetime.Singleton);
            container.RegisterType<UserNotificationMapper>(TypeLifetime.Singleton);
            container.RegisterType<TrainingMapper>(TypeLifetime.Singleton);

            container.RegisterType<IAccountDAL, AccountDAL>(TypeLifetime.Singleton);
            container.RegisterType<IDepartmentDAL, DepartmentDAL>(TypeLifetime.Singleton);
            container.RegisterType<IEmployeeDAL, EmployeeDAL>(TypeLifetime.Singleton);
            container.RegisterType<IEmployeeUploadDAL, EmployeeUploadDAL>(TypeLifetime.Singleton);
            container.RegisterType<IEnrollmentDAL, EnrollmentDAL>(TypeLifetime.Singleton);
            container.RegisterType<IUserNotificationDAL, EnrollmentNotificationDAL>(TypeLifetime.Singleton);
            container.RegisterType<IPrerequisiteDAL, PrerequisiteDAL>(TypeLifetime.Singleton);
            container.RegisterType<IUserNotificationDAL, EnrollmentNotificationDAL>(TypeLifetime.Singleton);
            container.RegisterType<ITrainingDAL, TrainingDAL>(TypeLifetime.Singleton);

            container.RegisterType<IAccountRepository, AccountRepository>(TypeLifetime.Singleton);
            container.RegisterType<IDepartmentRepository, DepartmentRepository>(TypeLifetime.Singleton);
            container.RegisterType<IEmployeeRepository, EmployeeRepository>(TypeLifetime.Singleton);
            container.RegisterType<IEnrollmentRepository, EnrollmentRepository>(TypeLifetime.Singleton);
            container.RegisterType<IUserNotificationRepository, UserNotificationRepository>(TypeLifetime.Singleton);
            container.RegisterType<IPrerequisiteRepository, PrerequisiteRepository>(TypeLifetime.Singleton);
            container.RegisterType<IUserNotificationRepository, UserNotificationRepository>(TypeLifetime.Singleton);
            container.RegisterType<ITrainingRepository, TrainingRepository>(TypeLifetime.Singleton);

            container.RegisterType<IAccountService, AccountService>(TypeLifetime.Singleton);
            container.RegisterType<IEmployeeService, EmployeeService>(TypeLifetime.Singleton);
            container.RegisterType<IEnrollmentService, EnrollmentService>(TypeLifetime.Singleton);
            container.RegisterType<INotificationService, NotificationService>(TypeLifetime.Singleton);
            container.RegisterType<IReportGenerationService, ExcelReportGenerationService>(TypeLifetime.Singleton);
            container.RegisterType<ITrainingService, TrainingService>(TypeLifetime.Singleton);
        }
    }
}