using Microsoft.EntityFrameworkCore;
using PersonalProjectApi.Data;
using PersonalProjectApi.Models.Domain;
using PersonalProjectApi.Models.DTO.CardDTOs;

namespace PersonalProjectApi.Repositories.CardsRepositories
{
    public class SQLCardsRepository : ICardsRepository
    {
        private readonly TaskBoardDbContext dbContext;
        public SQLCardsRepository(TaskBoardDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<List<Card>> GetAllCardsAsync()
        {
            var cardsDomain = await dbContext.Cards
                .Include(c => c.Priority)
                .Include(c => c.BoardList)
                .ToListAsync();

            return cardsDomain;
        }
    }
}
