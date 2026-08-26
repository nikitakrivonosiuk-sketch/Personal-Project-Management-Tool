using Microsoft.EntityFrameworkCore;
using PersonalProjectApi.Data;
using PersonalProjectApi.Models.Domain;

namespace PersonalProjectApi.Repositories.ActivityLogRepositories
{
    public class SQLActivityLogRepository : IActivityLogRepository
    {
        private readonly TaskBoardDbContext dbContext;
        public SQLActivityLogRepository(TaskBoardDbContext taskBoardDbContext)
        {
            this.dbContext = taskBoardDbContext;
        }

        public async Task<List<ActivityLog>> GetLogsAsync(Guid? cardId, int skip = 0, int take = 10)
        {
            var query = dbContext.ActivityLogs.AsQueryable();

            if (cardId.HasValue)
            {
                query = query.Where(log => log.CardId == cardId.Value);
            }

            return await query.OrderByDescending(log => log.CreatedAt)
                              .Skip(skip)
                              .Take(take)
                              .ToListAsync();
        }
    }
}
