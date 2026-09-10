using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PersonalProjectApi.Models.Domain;
using PersonalProjectApi.Models.DTO.BoardListsDto;
using PersonalProjectApi.Models.DTO.BoardsDto;
using PersonalProjectApi.Models.DTO.CardDTOs;
using PersonalProjectApi.Repositories.BoardRepository;

namespace PersonalProjectApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BoardsController : ControllerBase
    {
        private readonly IBoardRepository _boardsRepository;
        public BoardsController(IBoardRepository boardRepository)
        {
            _boardsRepository = boardRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBoards()
        {
            var boardsDomain = await _boardsRepository.GetAllBoardsAsync();

            var boardsDto = boardsDomain.Select(b => new Board
            {
                Id = b.Id,
                Title = b.Title,
                CreatedAt = b.CreatedAt,
            });

            return Ok(boardsDto);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetBoard(Guid id)
        {
            var boardDomain = await _boardsRepository.GetBoardAsync(id);

            if (boardDomain == null)
            {
                return NotFound("No board was found with this id.");
            }

            var boardDto = new BoardDto
            {
                Id = id,
                Title = boardDomain.Title,
                CreatedAt = boardDomain.CreatedAt,
                BoardLists = boardDomain.BoardLists.Select(l => new BoardListDto
                {
                    Id = l.Id,
                    Title = l.Title,
                    Position = l.Position,
                    Cards = l.Cards.Select(c => new CardDto
                    {
                        Id = c.Id,
                        Title = c.Title,
                        Priority = c.Priority,
                        Description = c.Description,
                        DueDate = c.DueDate,
                        BoardListId = l.Id,
                    }).ToList()
                }).ToList()
            };

            return Ok(boardDto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBoard([FromBody] CreatingBoardDto creatingRequestDto)
        {
            var boardDomain = new Board
            {
                Title = creatingRequestDto.Title,
                CreatedAt = DateTime.Now,
            };

            boardDomain = await _boardsRepository.CreateBoardAsync(boardDomain);

            if (boardDomain == null)
            {
                NotFound("Something went wrong while adding to DB.");
            }

            var boardDto = new BoardDto
            {
                Id = boardDomain.Id,
                Title = boardDomain.Title,
                CreatedAt = boardDomain.CreatedAt,
            };

            return Ok(boardDto);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateBoard([FromBody] UpdateBoardRequestDto requestDto)
        {
            var boardDomain = new Board
            {
                Id = requestDto.Id,
                Title = requestDto.Title,
            };

            boardDomain = await _boardsRepository.UpdateBoardAsync(boardDomain);

            if (boardDomain == null)
            {
                return NotFound("Could not find this board in DB.");
            }

            var boardDto = new BoardDto
            {
                Id = boardDomain.Id,
                Title = boardDomain.Title,
            };

            return Ok(boardDto);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteBoard([FromRoute] Guid id)
        {
            var isDeleted = await _boardsRepository.DeleteBoardAsync(id);

            if (!isDeleted)
            {
                return NotFound("Board was not found in DB.");
            }

            return Ok("Successfully deleted.");
        }
    }
}
