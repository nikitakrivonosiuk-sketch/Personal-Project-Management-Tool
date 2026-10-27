using PersonalProjectApi.Models.Domain;
using PersonalProjectApi.Models.DTO.CardDTOs;
using PersonalProjectApi.Repositories.CardsRepositories;

namespace PersonalProjectApi.Services
{
    public class CardsService
    {
        private ICardsRepository _cardsRepository;

        public CardsService(ICardsRepository cardsRepository)
        {
            _cardsRepository = cardsRepository;
        }

        public async Task<List<CardDto>> GetAllCardsAsync()
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
            }).ToList();

            return cardsDto;
        }

        public async Task<CardDto> CreateCardAsync(AddCardRequestDto cardRequestDto)
        {
            var cardDomain = new Card
            {
                Title = cardRequestDto.Title,
                Description = cardRequestDto.Description,
                DueDate = cardRequestDto.DueDate,
                Priority = cardRequestDto.Priority,
                BoardListId = cardRequestDto.BoardListId,
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

            return cardDto;
        }

        public async Task<CardDto> UpdateCardAsync(Guid id, UpdateCardDto cardRequestDto)
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
                throw new KeyNotFoundException($"Card with id {id} was not found.");
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

            return cardDto;
        }

        public async Task<string> DeleteCardAsync(Guid id)
        {
            var isDeleted = await _cardsRepository.DeleteCardAsync(id);

            if (!isDeleted)
            {
                throw new KeyNotFoundException($"Card with id {id} was not deleted. Invalid Id.");
            }

            return "Deleted succesfully";
        }
    }
}
