using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.OpenApi.Validations;
using PersonalProjectApi.Models.Domain;
using PersonalProjectApi.Models.DTO.BoardListsDto;
using PersonalProjectApi.Repositories.ListRepositories;

namespace PersonalProjectApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListsController : ControllerBase
    {
        private readonly IListsRepository _listsRepository;
        public ListsController(IListsRepository repository)
        {
            this._listsRepository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetLists()
        {
            var listsDomain = await _listsRepository.GetAllBoardListsAsync();

            var listsDto = listsDomain.Select(list => new BoardListDto
            {
                Id = list.Id,
                Title = list.Title,
                Position = list.Position,
            });

            return Ok(listsDto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateList([FromBody] CreateListRequestDto requestDto)
        {
            var listDomain = new BoardList
            {
                Title = requestDto.Title,
                BoardId = requestDto.BoardId,
            };

            listDomain = await _listsRepository.CreateListAsync(listDomain);

            var listDto = new BoardListDto
            {
                Id = listDomain.Id,
                Title = listDomain.Title,
            };

            return Ok(listDto);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateList([FromRoute] Guid id, [FromBody] BoardListDto boardListDto)
        {
            var listDomain = new BoardList
            {
                Title= boardListDto.Title,
                Position = boardListDto.Position,
            };

            listDomain = await _listsRepository.UpdateListAsync(id, listDomain);

            if (listDomain == null)
            {
                return NotFound(boardListDto);
            }

            var listDto = new BoardListDto
            {
                Id = listDomain.Id,
                Title = listDomain.Title,
                Position = listDomain.Position,
            };

            return Ok(listDto);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteList([FromRoute] Guid id)
        {
            var isDeleted = await _listsRepository.DeleteListAsync(id);

            if (!isDeleted)
            {
                return NotFound("List was not found.");
            }

            return Ok("Succesfully deleted.");
        }
    }
}
