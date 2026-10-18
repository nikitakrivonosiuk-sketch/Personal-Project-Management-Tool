using Azure.Core;
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
        private readonly ILogger<LogsController> _logger;
        public LogsController(IActivityLogRepository logRepository, ILogger<LogsController> logger)
        {
            _activityLogRepository = logRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetLogs(
        [FromQuery] Guid? boardId,
        [FromQuery] Guid? listId,
        [FromQuery] Guid? cardId,
        [FromQuery] int skip = 0,
        [FromQuery] int take = 10)
        {
            _logger.LogInformation($"{nameof(GetLogs)} received request: {System.Text.Json.JsonSerializer.Serialize(new { boardId, listId, cardId, skip, take })}");

            var logs = await _activityLogRepository.GetLogsAsync(boardId, listId, cardId, skip, take);
            return Ok(logs);
        }
    }
}
