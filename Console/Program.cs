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

namespace Console.DIConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Create Unity container
            IUnityContainer container = new UnityContainer();

            // Register types with the container
            container.RegisterType<DbUtil>
            (
                TypeLifetime.Singleton,
                new InjectionConstructor(ConfigurationManager.ConnectionStrings["DefaultConnectionString"].ConnectionString)
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

            // Resolve instances of types needed
            IUserService userService = container.Resolve<IUserService>();

            // Demo
            // Register new user
            User newUser = new User()
            {
                Role = UserRoleEnum.Employee,
                Email = "joe@gmail.com",
                Password = "password",
                Name = "Joe Doe",
                NIC = "JD123",
                Phone = "1234567890",
                DeptID = 1,
                ManagerID = 2
            };

            bool isRegisteredUser = userService.Register(newUser);
            System.Console.WriteLine($"User with ID: {isRegisteredUser} created.");

            try
            {
                bool newUserID2 = userService.Register(newUser);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.ToString());
                //throw; // Preserve call stack. || throw ex; -> throw at this level
            }

            // Login
            User loginUser = userService.Login(1, newUser.Password);
            System.Console.WriteLine($"User with ID: {loginUser.ID} and Name: {loginUser.Name} successfully logged in.");

            try
            {
                User loginUser2 = userService.Login(1, "WRONG PASSWORD");
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.ToString());
            }

            System.Console.ReadLine();
        }
    }
}
