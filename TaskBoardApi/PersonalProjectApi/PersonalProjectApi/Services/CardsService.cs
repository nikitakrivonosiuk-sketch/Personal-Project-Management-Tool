using PersonalProjectApi.Models.Domain;
using PersonalProjectApi.Models.DTO.CardDTOs;
using PersonalProjectApi.Repositories.CardsRepositories;
using PersonalProjectApi.Repositories.ListRepositories;
using PersonalProjectApi.Services.Authorization;

namespace PersonalProjectApi.Services
{
    public class CardsService
    {
        private readonly ICardsRepository _cardsRepository;
        private readonly IListsRepository _listsRepository;
        private readonly BoardAuthorizationService _authService;

        public CardsService(ICardsRepository cardsRepository,
            IListsRepository listsRepository,
            BoardAuthorizationService boardAuthorizationService)
        {
            _cardsRepository = cardsRepository;
            _authService = boardAuthorizationService;
            _listsRepository = listsRepository;
        }

        public async Task<List<CardDto>> GetAllCardsAsync(Guid userId)
        {
            var cardsDomain = await _cardsRepository.GetAllCardsAsync(userId);

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

        public async Task<CardDto> CreateCardAsync(AddCardRequestDto cardRequestDto, Guid userId)
        {
            var listWithMembers = await _listsRepository.GetListWithBoardMembersAsync(cardRequestDto.BoardListId);

            _authService.EnsureCanEdit(userId, listWithMembers.Board);

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

        public async Task<CardDto> UpdateCardAsync(Guid cardId, UpdateCardDto cardRequestDto, Guid userId)
        {
            var cardWithMembers = await _cardsRepository.GetCardWithBoardMembersAsync(cardId);

            _authService.EnsureCanEdit(userId, cardWithMembers.BoardList.Board);

            var cardDomain = new Card
            {
                Title = cardRequestDto.Title,
                Description = cardRequestDto.Description,
                DueDate = cardRequestDto.DueDate,
                Priority = cardRequestDto.Priority,
                BoardListId = cardWithMembers.BoardListId,
            };

            cardDomain = await _cardsRepository.UpdateCardAsync(cardId, cardDomain);

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

        public async Task<string> DeleteCardAsync(Guid cardId, Guid userId)
        {
            var cardWithBoardMembers = await _cardsRepository.GetCardWithBoardMembersAsync(cardId);
            
            _authService.EnsureCanEdit(userId, cardWithBoardMembers.BoardList.Board);

            var isDeleted = await _cardsRepository.DeleteCardAsync(cardId);

            if (!isDeleted)
            {
                throw new KeyNotFoundException($"Card with id {cardId} was not deleted. Invalid Id.");
            }

            return "Deleted succesfully";
        }
    }
}
