using WorkNestify.DataAccess.Entities.Users;

namespace WorkNestify.DataAccess.Repositories.Interfaces.Users;

public interface IJobSeekerRepository : IRepository<JobSeeker>
{
    Task UpdateAsync(JobSeeker jobSeeker);
}