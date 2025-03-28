using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WorkNestify.DataAccess.Repositories.Interfaces;
using WorkNestify.Utilities.Constants;

namespace WorkNestify.Services
{
    public class JobStatusUpdateService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<JobStatusUpdateService> _logger;
        private readonly TimeSpan _checkInterval = TimeSpan.FromHours(1);

        public JobStatusUpdateService(
            IServiceProvider serviceProvider,
            ILogger<JobStatusUpdateService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await UpdateJobStatuses();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while updating job statuses");
                }

                await Task.Delay(_checkInterval, stoppingToken);
            }
        }

        private async Task UpdateJobStatuses()
        {
            using var scope = _serviceProvider.CreateScope();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

            var currentDate = DateTime.UtcNow;
            var jobsToExpire = await unitOfWork.Jobs
                .GetAllQueryable(j => 
                    j.Status != JobStatuses.Expired &&
                    j.EndDate.HasValue &&
                    j.EndDate.Value <= currentDate)
                .ToListAsync();

            foreach (var job in jobsToExpire)
            {
                job.Status = JobStatuses.Expired;
                job.ModifiedDate = currentDate;
                await unitOfWork.Jobs.UpdateAsync(job);
            }

            if (jobsToExpire.Any())
            {
                await unitOfWork.SaveAsync();
                _logger.LogInformation($"Updated {jobsToExpire.Count} jobs to Expired status");
            }
        }
    }
}