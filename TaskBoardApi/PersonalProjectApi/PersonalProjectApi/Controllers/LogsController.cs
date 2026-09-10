using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PersonalProjectApi.Models.DTO.ActivityLogDtos;
using PersonalProjectApi.Repositories.ActivityLogRepositories;
using PersonalProjectApi.Repositories.ListRepositories;

namespace PersonalProjectApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogsController : ControllerBase
    {
        private readonly IActivityLogRepository _activityLogRepository;
        public LogsController(IActivityLogRepository logRepository)
        {
            _activityLogRepository = logRepository;
        }

        [HttpGet("{boardId:guid}/logs")]
        public async Task<IActionResult> GetBoardLogs([FromRoute] Guid boardId, [FromQuery] int skip = 0, [FromQuery] int take = 10)
        {
            var logs = await _activityLogRepository.GetLogsAsync(boardId: boardId, skip: skip, take: take);

            var logsDto = logs.Select(log => new ActivityLogDto
            {
                Id = log.Id,
                CardId = log.CardId,
                BoardListId = log.BoardListId,
                BoardId = log.BoardId,
                ActionType = log.ActionType,
                Description = log.Description,
                CreatedAt = log.CreatedAt,
            }).ToList();

            return Ok(logsDto);
        }

    }
}
