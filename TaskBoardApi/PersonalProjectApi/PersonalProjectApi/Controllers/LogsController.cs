using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalProjectApi.Repositories.ActivityLogRepositories;
using PersonalProjectApi.Services.Authorization;
using System.Drawing.Printing;
using System.Text.Json;

namespace PersonalProjectApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LogsController : ControllerBase
    {
        private readonly IActivityLogRepository _activityLogRepository;
        private readonly ILogger<LogsController> _logger;
        private readonly CurrentUserService _currentUserService;
        public LogsController(IActivityLogRepository logRepository,
            ILogger<LogsController> logger,
            CurrentUserService currentUserService)
        {
            _activityLogRepository = logRepository;
            _logger = logger;
            _currentUserService = currentUserService;
        }

        [HttpGet]
        public async Task<IActionResult> GetLogs(
        [FromQuery] Guid? boardId,
        [FromQuery] Guid? listId,
        [FromQuery] Guid? cardId,
        [FromQuery] int skip = 0,
        [FromQuery] int take = 10)
        {
            _logger.LogInformation($"{nameof(GetLogs)} received request: {JsonSerializer.Serialize(new { boardId, listId, cardId, skip, take, _currentUserService.UserId})}");

            var logs = await _activityLogRepository.GetLogsAsync(_currentUserService.UserId, boardId, listId, cardId, skip, take);
            return Ok(logs);
        }
    }
}
