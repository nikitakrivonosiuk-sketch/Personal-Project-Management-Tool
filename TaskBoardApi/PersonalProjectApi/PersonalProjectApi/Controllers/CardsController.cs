using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PersonalProjectApi.Models.Domain;
using PersonalProjectApi.Models.DTO.CardDTOs;
using PersonalProjectApi.Repositories.CardsRepositories;
using PersonalProjectApi.Services;

namespace PersonalProjectApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardsController : ControllerBase
    {
        private readonly CardsService _cardsService;

        public CardsController(CardsService cardsService)
        {
            _cardsService = cardsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCards() 
        {
            var cards = await _cardsService.GetAllCardsAsync();
            return Ok(cards);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCard([FromBody] AddCardRequestDto cardRequestDto)
        {
            var card = await _cardsService.CreateCardAsync(cardRequestDto); 
            return Ok(card);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateCard([FromRoute] Guid id, [FromBody] UpdateCardDto cardRequestDto)
        {
            var card = await _cardsService.UpdateCardAsync(id, cardRequestDto);
            return Ok(card);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteCard([FromRoute] Guid id)
        {
            var deleted = await _cardsService.DeleteCardAsync(id);
            return Ok(deleted);
        }
    }
}
