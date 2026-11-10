using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalProjectApi.Models.DTO.CardDTOs;
using PersonalProjectApi.Services;
using PersonalProjectApi.Services.Authorization;
using System.Text.Json;

namespace PersonalProjectApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CardsController : ControllerBase
    {
        private readonly CardsService _cardsService;
        private readonly ILogger<CardsController> _logger;
        private readonly CurrentUserService _currentUserService;

        public CardsController(CardsService cardsService, 
            ILogger<CardsController> logger,
            CurrentUserService currentUserService)
        {
            _cardsService = cardsService;
            _logger = logger;
            _currentUserService = currentUserService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCards() 
        {
            _logger.LogInformation($"{nameof(GetAllCards)} received request.");

            var cards = await _cardsService.GetAllCardsAsync(_currentUserService.UserId);
            return Ok(cards);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCard([FromBody] AddCardRequestDto cardRequestDto)
        {
            _logger.LogInformation($"{nameof(CreateCard)} received request: {JsonSerializer.Serialize(cardRequestDto)}");

            var card = await _cardsService.CreateCardAsync(cardRequestDto, _currentUserService.UserId); 
            return Ok(card);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateCard([FromRoute] Guid id, [FromBody] UpdateCardDto cardRequestDto)
        {
            _logger.LogInformation($"{nameof(UpdateCard)} received request: {JsonSerializer.Serialize(cardRequestDto)}, id: {id}");

            var card = await _cardsService.UpdateCardAsync(id, cardRequestDto, _currentUserService.UserId);
            return Ok(card);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteCard([FromRoute] Guid id)
        {
            _logger.LogInformation($"{nameof(DeleteCard)} received request: {id}");

            var deleted = await _cardsService.DeleteCardAsync(id, _currentUserService.UserId);
            return Ok(deleted);
        }
    }
}
