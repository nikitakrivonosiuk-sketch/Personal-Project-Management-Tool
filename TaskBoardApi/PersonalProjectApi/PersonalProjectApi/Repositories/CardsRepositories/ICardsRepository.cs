using PersonalProjectApi.Models.Domain;
using PersonalProjectApi.Models.DTO.CardDTOs;

namespace PersonalProjectApi.Repositories.CardsRepositories
{
    public interface ICardsRepository
    {
        Task<List<Card>> GetAllCardsAsync();
        Task<Card> CreateCardAsync(Card card);
        Task<Card> UpdateCardAsync(Guid id, Card card);
        Task<Boolean> DeleteCardAsync(Guid id);
    }
}
