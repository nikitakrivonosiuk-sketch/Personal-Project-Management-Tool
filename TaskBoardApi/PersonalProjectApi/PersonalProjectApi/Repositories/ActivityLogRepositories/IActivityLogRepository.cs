using PersonalProjectApi.Models.Domain;

namespace PersonalProjectApi.Repositories.ActivityLogRepositories
{
    public interface IActivityLogRepository
    {
        Task<List<ActivityLog>> GetLogsAsync(Guid? boardId = null,Guid? listId = null, Guid? cardId = null, int skip = 0, int take = 10);
    }
}
