using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalProjectApi.Models.Domain;
using PersonalProjectApi.Models.DTO.BoardsDto;
using PersonalProjectApi.Services;
using PersonalProjectApi.Services.Authorization;
using System.Text.Json;

namespace PersonalProjectApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BoardsController : ControllerBase
    {
        private readonly BoardService _boardService;
        private readonly ILogger<BoardsController> _logger;
        private readonly CurrentUserService _currentUser;

        public BoardsController(BoardService boardService, ILogger<BoardsController> logger, CurrentUserService currentUserService)
        {
            _currentUser = currentUserService;
            _boardService = boardService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBoards([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 8)
        {
            _logger.LogInformation($"{nameof(GetAllBoards)} received request.");

            var boards = await _boardService.GetAllBoards(_currentUser.UserId, pageNumber, pageSize);
            return Ok(boards);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetBoard(Guid id)
        {
            _logger.LogInformation($"{nameof(GetBoard)} received request: {id}");

            var board = await _boardService.GetBoardByIdAsync(_currentUser.UserId, id);
            return Ok(board);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBoard([FromBody] CreatingBoardDto creatingRequestDto)
        {
            _logger.LogInformation($"{nameof(CreateBoard)} received request: {JsonSerializer.Serialize(creatingRequestDto)}");

            var board = await _boardService.CreateBoardAsync(_currentUser.UserId, creatingRequestDto);
            return Ok(board);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateBoard(Guid id, [FromBody] UpdateBoardRequestDto requestDto)
        {
            _logger.LogInformation($"{nameof(UpdateBoard)} received request: {JsonSerializer.Serialize(requestDto)}");

            var board = await _boardService.UpdateBoardAsync(id, requestDto, _currentUser.UserId);
            return Ok(board);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteBoard([FromRoute] Guid id)
        {
            _logger.LogInformation($"{nameof(DeleteBoard)} received request: {id}");

            var deleted = await _boardService.DeleteBoardAsync(id, _currentUser.UserId);
            return Ok(deleted);
        }
    }
}
