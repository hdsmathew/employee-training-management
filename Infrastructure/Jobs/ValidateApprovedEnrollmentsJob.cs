using Core.Application;
using Core.Application.Models;
using Core.Application.Services;
using Quartz;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Jobs
{
    public class ValidateApprovedEnrollmentsJob : IJob
    {
        private readonly IEnrollmentService _enrollmentService;
        private readonly ILogger _logger;

        public ValidateApprovedEnrollmentsJob(IEnrollmentService enrollmentService, ILogger logger)
        {
            _enrollmentService = enrollmentService;
            _logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var results = await _enrollmentService.ValidateApprovedEnrollmentsAsync(null);

            StringBuilder logEntry = new StringBuilder($"Job Run @{DateTime.Now} - Status: {results.IsSuccess}\n");

            if (results.IsSuccess)
            {
                foreach ((string trainingName, Result enrollmentValidationResult) in results.Value)
                {
                    logEntry.AppendLine($"Training: {trainingName} - Enrollment Validation: {enrollmentValidationResult}");
                }
            }
            else
            {
                logEntry.AppendLine($"Job Error: {results.Error}");
            }
            logEntry.AppendLine("--------------------------------------------------");
            _logger.Log(logEntry.ToString());
        }
    }
}
