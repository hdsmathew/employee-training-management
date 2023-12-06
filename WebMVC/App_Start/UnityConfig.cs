using Core.Application.Repositories;
using Core.Application.Services;
using Core.Domain.Enrollment;
using Core.Domain.Training;
using Core.Domain.User;
using Infrastructure.DAL;
using Infrastructure.Repositories;
using System;
using System.Configuration;
using Unity;
using Unity.Injection;

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

            container.RegisterType<DbUtil>
            (
                TypeLifetime.Singleton,
                new InjectionConstructor(ConfigurationManager.AppSettings["DefaultConnectionString"])
            );

            container.RegisterType<UserMapper>(TypeLifetime.Singleton);
            container.RegisterType<TrainingMapper>(TypeLifetime.Singleton);
            container.RegisterType<EnrollmentMapper>(TypeLifetime.Singleton);

            container.RegisterType<IUserDAL, UserDAL>(TypeLifetime.Singleton);
            container.RegisterType<ITrainingDAL, TrainingDAL>(TypeLifetime.Singleton);
            container.RegisterType<IEnrollmentDAL, EnrollmentDAL>(TypeLifetime.Singleton);

            container.RegisterType<IUserRepository, UserRepository>(TypeLifetime.Singleton);
            container.RegisterType<ITrainingRepository, TrainingRepository>(TypeLifetime.Singleton);
            container.RegisterType<IEnrollmentRepository, EnrollmentRepository>(TypeLifetime.Singleton);

            container.RegisterType<IUserService, UserService>(TypeLifetime.Singleton);
            container.RegisterType<ITrainingService, TrainingService>(TypeLifetime.Singleton);
            container.RegisterType<IEnrollmentService, EnrollmentService>(TypeLifetime.Singleton);
        }
    }
}