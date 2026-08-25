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

        public async Task<Card> CreateCardAsync(Card card)
        {
            await dbContext.Cards.AddAsync(card);
            await dbContext.SaveChangesAsync();
            return card;
        }

        public async Task<List<Card>> GetAllCardsAsync()
        {
            var cardsDomain = await dbContext.Cards
                .Include(c => c.BoardList)
                .OrderBy(c => c.DueDate)
                .ToListAsync();
            return cardsDomain;
        }

        public async Task<Card> UpdateCardAsync(Guid Id, Card card)
        {
            var cardDomain = await dbContext.Cards.FirstOrDefaultAsync(c => c.Id == Id);

            if (cardDomain == null)
            {
                return null;
            }

            cardDomain.Title = card.Title;
            cardDomain.Description = card.Description;
            cardDomain.DueDate = card.DueDate;
            cardDomain.BoardList = card.BoardList;
            cardDomain.Priority = card.Priority;
            cardDomain.BoardListId = card.BoardListId;

            await dbContext.SaveChangesAsync();

            return cardDomain;
        }

        public async Task<Boolean> DeleteCardAsync(Guid id)
        {
            var deletedCards = await dbContext.Cards
                .Where(c => c.Id == id)
                .ExecuteDeleteAsync();

            if (deletedCards == 0)
            {
                return false;
            }

            return true;
        }
    }
}
