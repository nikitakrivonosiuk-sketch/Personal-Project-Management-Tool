using PersonalProjectApi.Models.Domain;

namespace PersonalProjectApi.Repositories.ActivityLogRepositories
{
    public interface IActivityLogRepository
    {
        Task<List<ActivityLog>> GetLogsAsync(Guid? cardId, int Skip, int Take);
    }
}
