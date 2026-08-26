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

        [HttpGet]
        public async Task<IActionResult> GetAllLogs([FromQuery] Guid? cardId, [FromQuery] int skip = 0, [FromQuery] int take = 10)
        {
            var logsDomain = await _activityLogRepository.GetLogsAsync(cardId, skip, take);

            var logsDto = logsDomain.Select(log => new ActivityLogDto
            {
                Id = log.Id,
                CardId = log.CardId,
                ActionType = log.ActionType,
                Description = log.Description,
                CreatedAt = log.CreatedAt,
            }).ToList();

            return Ok(logsDto);
        }
    }
}
