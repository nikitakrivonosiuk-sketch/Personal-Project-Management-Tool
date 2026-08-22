using PersonalProjectApi.Models.Domain;
using PersonalProjectApi.Models.DTO.CardDTOs;

namespace PersonalProjectApi.Repositories.CardsRepositories
{
    public interface ICardsRepository
    {
        Task<List<Card>> GetAllCardsAsync();
    }
}
