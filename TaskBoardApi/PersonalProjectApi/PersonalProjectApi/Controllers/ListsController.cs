using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalProjectApi.Models.DTO.BoardListsDto;
using PersonalProjectApi.Services;
using PersonalProjectApi.Services.Authorization;
using System.Text.Json;

namespace PersonalProjectApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ListsController : ControllerBase
    {
        private readonly ListService _listService;
        private readonly ILogger<ListsController> _logger;
        private readonly CurrentUserService _currentUserService;
        public ListsController(ListService listService, ILogger<ListsController> logger, CurrentUserService currentUserService)
        {
            _listService = listService;
            _logger = logger;
            _currentUserService = currentUserService;
        }

        [HttpGet]
        public async Task<IActionResult> GetLists()
        {
            _logger.LogInformation($"{nameof(GetLists)} received request.");

            var lists = await _listService.GetListsAsync(_currentUserService.UserId);
            return Ok(lists);
        }

        [HttpPost]
        public async Task<IActionResult> CreateList([FromBody] CreateListRequestDto requestDto)
        {
            _logger.LogInformation($"{nameof(CreateList)} received request: {JsonSerializer.Serialize(requestDto)}");

            var list = await _listService.CreateListAsync(_currentUserService.UserId, requestDto);
            return Ok(list);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateList([FromRoute] Guid id, [FromBody] BoardListDto boardListDto)
        {
            _logger.LogInformation($"{nameof(UpdateList)} received request: {JsonSerializer.Serialize(boardListDto)}");

            var list = await _listService.UpdateListASync(id, boardListDto, _currentUserService.UserId);
            return Ok(list);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteList([FromRoute] Guid id)
        {
            _logger.LogInformation($"{nameof(DeleteList)} received request: {id}");

            var deleted = await _listService.DeleteListAsync(id, _currentUserService.UserId);
            return Ok(deleted);
        }
    }
}
