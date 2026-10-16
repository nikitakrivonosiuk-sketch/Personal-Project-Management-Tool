using Azure.Core;
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
        private readonly ILogger<CardsController> _logger;

        public CardsController(CardsService cardsService, ILogger<CardsController> logger)
        {
            _cardsService = cardsService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCards() 
        {
            _logger.LogInformation($"{nameof(GetAllCards)} received request.");

            var cards = await _cardsService.GetAllCardsAsync();
            return Ok(cards);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCard([FromBody] AddCardRequestDto cardRequestDto)
        {
            _logger.LogInformation($"{nameof(CreateCard)} received request: {System.Text.Json.JsonSerializer.Serialize(cardRequestDto)}");

            var card = await _cardsService.CreateCardAsync(cardRequestDto); 
            return Ok(card);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateCard([FromRoute] Guid id, [FromBody] UpdateCardDto cardRequestDto)
        {
            _logger.LogInformation($"{nameof(UpdateCard)} received request: {System.Text.Json.JsonSerializer.Serialize(cardRequestDto)}, id: {id}");

            var card = await _cardsService.UpdateCardAsync(id, cardRequestDto);
            return Ok(card);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteCard([FromRoute] Guid id)
        {
            _logger.LogInformation($"{nameof(DeleteCard)} received request: {id}");

            var deleted = await _cardsService.DeleteCardAsync(id);
            return Ok(deleted);
        }
    }
}
