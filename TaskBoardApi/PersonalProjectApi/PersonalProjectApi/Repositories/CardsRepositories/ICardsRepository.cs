using PersonalProjectApi.Models.Domain;
using PersonalProjectApi.Models.DTO.CardDTOs;
using PersonalProjectApi.Repositories.Common;

namespace PersonalProjectApi.Repositories.CardsRepositories
{
    public interface ICardsRepository : IRepository<Card>
    {
        Task<List<Card>> GetAllCardsAsync();
        Task<Card> CreateCardAsync(Card card);
        Task<Card> UpdateCardAsync(Guid id, Card card);
        Task<Boolean> DeleteCardAsync(Guid id);
    }
}
