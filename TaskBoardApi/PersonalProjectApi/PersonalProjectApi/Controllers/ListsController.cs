using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.OpenApi.Validations;
using PersonalProjectApi.Models.Domain;
using PersonalProjectApi.Models.DTO.BoardListsDto;
using PersonalProjectApi.Repositories.ListRepositories;
using PersonalProjectApi.Services;

namespace PersonalProjectApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListsController : ControllerBase
    {
        private readonly ListService _listService;
        public ListsController(ListService listService)
        {
            _listService = listService;
        }

        [HttpGet]
        public async Task<IActionResult> GetLists()
        {
            var lists = await _listService.GetListsAsync();
            return Ok(lists);
        }

        [HttpPost]
        public async Task<IActionResult> CreateList([FromBody] CreateListRequestDto requestDto)
        {
            var list = await _listService.CreateListAsync(requestDto);
            return Ok(list);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateList([FromRoute] Guid id, [FromBody] BoardListDto boardListDto)
        {
            var list = _listService.UpdateListASync(id, boardListDto);
            return Ok(list);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteList([FromRoute] Guid id)
        {
            var deleted = _listService.DeleteListAsync(id);
            return Ok(deleted);
        }
    }
}
