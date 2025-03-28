using WorkNestify.Models.Models.Users;

namespace WorkNestify.DataAccess.Repositories.Interfaces.Users;

public interface ISavedJob : IRepository<SavedJob>
{
    Task UpdateAsync(SavedJob job);
}