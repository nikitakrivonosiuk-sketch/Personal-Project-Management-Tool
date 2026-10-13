using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PersonalProjectApi.Models.Domain;
using PersonalProjectApi.Models.DTO.BoardListsDto;
using PersonalProjectApi.Models.DTO.BoardsDto;
using PersonalProjectApi.Models.DTO.CardDTOs;
using PersonalProjectApi.Repositories.BoardRepository;
using PersonalProjectApi.Services;

namespace PersonalProjectApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BoardsController : ControllerBase
    {
        private readonly BoardService _boardService;

        public BoardsController(BoardService boardService)
        {
            _boardService = boardService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBoards()
        {
            var boards = await _boardService.GetAllBoards();
            return Ok(boards);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetBoard(Guid id)
        {
            var board = await _boardService.GetBoardByIdAsync(id);
            return Ok(board);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBoard([FromBody] CreatingBoardDto creatingRequestDto)
        {
            var board = await _boardService.CreateBoardAsync(creatingRequestDto);
            return Ok(board);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateBoard([FromBody] UpdateBoardRequestDto requestDto)
        {
            var board = await _boardService.UpdateBoardAsync(requestDto);
            return Ok(board);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteBoard([FromRoute] Guid id)
        {
            var deleted = _boardService.DeleteBoardAsync(id);
            return Ok(deleted);
        }
    }
}
