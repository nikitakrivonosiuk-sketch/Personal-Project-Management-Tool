using Microsoft.AspNetCore.Mvc;
using PersonalProjectApi.Models.DTO.BoardsDto;
using PersonalProjectApi.Services;

namespace PersonalProjectApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BoardsController : ControllerBase
    {
        private readonly BoardService _boardService;
        private readonly ILogger<BoardsController> _logger;

        public BoardsController(BoardService boardService, ILogger<BoardsController> logger)
        {
            _boardService = boardService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBoards([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 8)
        {
            _logger.LogInformation($"{nameof(GetAllBoards)} received request.");

            var boards = await _boardService.GetAllBoards(pageNumber, pageSize);
            return Ok(boards);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetBoard(Guid id)
        {
            _logger.LogInformation($"{nameof(GetBoard)} received request: {id}");

            var board = await _boardService.GetBoardByIdAsync(id);
            return Ok(board);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBoard([FromBody] CreatingBoardDto creatingRequestDto)
        {
            _logger.LogInformation($"{nameof(CreateBoard)} received request: {System.Text.Json.JsonSerializer.Serialize(creatingRequestDto)}");

            var board = await _boardService.CreateBoardAsync(creatingRequestDto);
            return Ok(board);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateBoard([FromBody] UpdateBoardRequestDto requestDto)
        {
            _logger.LogInformation($"{nameof(UpdateBoard)} received request: {System.Text.Json.JsonSerializer.Serialize(requestDto)}");

            var board = await _boardService.UpdateBoardAsync(requestDto);
            return Ok(board);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteBoard([FromRoute] Guid id)
        {
            _logger.LogInformation($"{nameof(DeleteBoard)} received request: {id}");

            var deleted = await _boardService.DeleteBoardAsync(id);
            return Ok(deleted);
        }
    }
}
