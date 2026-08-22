using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PersonalProjectApi.Models.DTO.BoardListsDto;
using PersonalProjectApi.Repositories.ListRepositories;

namespace PersonalProjectApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListsController : ControllerBase
    {
        private readonly IListsRepository repository;
        public ListsController(IListsRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllLists()
        {
            var listsDomain = await repository.GetAllBoardListsAsync();

            var listsDto = listsDomain.Select(list => new BoardListDto
            {
                Id = list.Id,
                Title = list.Title,
            });

            return Ok(listsDto);
        }
    }
}
