using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PersonalProjectApi.Models.Domain;
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
                Priority = card.Priority,
                BoardListId = card.BoardListId,

                BoardTitle = card.BoardList.Title,
            });

            return Ok(cardsDto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCard([FromBody] AddCardRequestDto cardRequestDto)
        {
            var cardDomain = new Card
            {
                Title = cardRequestDto.Title,
                Description = cardRequestDto.Description,
                DueDate = cardRequestDto.DueDate,
                Priority = cardRequestDto.Priority,
                BoardListId= cardRequestDto.BoardListId,
            };

            cardDomain = await _cardsRepository.CreateCardAsync(cardDomain);

            var cardDto = new CardDto
            {
                Id = cardDomain.Id,
                Title = cardDomain.Title,
                Description = cardDomain.Description,
                DueDate = cardDomain.DueDate,
                Priority = cardDomain.Priority,
                BoardListId = cardDomain.BoardListId
            };

            return Ok(cardDto);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateCard([FromRoute] Guid id, [FromBody] UpdateCardDto cardRequestDto)
        {
            var cardDomain = new Card
            {
                Title = cardRequestDto.Title,
                Description = cardRequestDto.Description,
                DueDate = cardRequestDto.DueDate,
                Priority = cardRequestDto.Priority,
                BoardListId = cardRequestDto.BoardListId
            };

            cardDomain = await _cardsRepository.UpdateCardAsync(id, cardDomain);

            if (cardDomain == null)
            {
                return NotFound("Card was not found. Invalid Id.");
            }

            var cardDto = new CardDto
            {
                Id = cardDomain.Id,
                Title = cardDomain.Title,
                Description = cardDomain.Description,
                DueDate = cardDomain.DueDate,
                Priority = cardDomain.Priority,
                BoardListId = cardDomain.BoardListId,
            };

            return Ok(cardDto);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteCard([FromRoute] Guid id)
        {
            var isDeleted = await _cardsRepository.DeleteCardAsync(id);

            if (!isDeleted)
            {
                return NotFound("Card was not found. Invalid id.");
            }
            
            return Ok("Card was deleted succesfully.");
        }
    }
}
