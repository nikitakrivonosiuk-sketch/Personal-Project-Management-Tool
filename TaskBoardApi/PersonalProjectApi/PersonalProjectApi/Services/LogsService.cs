using PersonalProjectApi.Models.DTO.ActivityLogDtos;
using PersonalProjectApi.Repositories.ActivityLogRepositories;

namespace PersonalProjectApi.Services
{
    public class LogsService
    {
        private readonly IActivityLogRepository _activityLogRepository;

        public LogsService(IActivityLogRepository activityLogRepository)
        {
            _activityLogRepository = activityLogRepository;   
        }

        public async Task<List<ActivityLogDto>> GetLogs (Guid cardId, Guid listId, Guid boardId, int take, int skip)
        {
            var logs = await _activityLogRepository.GetLogsAsync(boardId, listId, cardId, skip, take);

            var logsDto = logs.Select(log => new ActivityLogDto
            {
                Id = log.Id,
                CardId = log.CardId,
                BoardListId = log.BoardListId,
                BoardId = log.BoardId,
                ActionType = log.ActionType,
                Description = log.Description,
                CreatedAt = log.CreatedAt,
            })
                .OrderBy(x => x.CreatedAt)
                .ToList();

            return logsDto;
        }
    }
}
