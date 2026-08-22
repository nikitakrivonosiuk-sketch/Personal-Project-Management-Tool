using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PersonalProjectApi.Models.DTO.CardDTOs;
using PersonalProjectApi.Repositories.CardsRepositories;

namespace PersonalProjectApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardsController : ControllerBase
    {
        private readonly ICardsRepository _cardsRepository;
        public CardsController(ICardsRepository repository)
        {
            _cardsRepository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCards() 
        {
            var cardsDomain = await _cardsRepository.GetAllCardsAsync();

            var cardsDto = cardsDomain.Select(card => new CardDto
            {
                Id = card.Id,
                Title = card.Title,
                Description = card.Description,
                DueDate = card.DueDate,
                PriorityId = card.PriorityId,
                BoardListId = card.BoardListId,

                BoardTitle = card.BoardList.Title,
                Priority = card.Priority.CardPriority,
            });

            return Ok(cardsDto);
        }


    }
}
