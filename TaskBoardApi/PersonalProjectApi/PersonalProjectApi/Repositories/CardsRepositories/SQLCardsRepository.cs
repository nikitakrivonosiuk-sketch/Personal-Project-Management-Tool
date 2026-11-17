using Microsoft.EntityFrameworkCore;
using PersonalProjectApi.Data;
using PersonalProjectApi.Models.Domain;
using PersonalProjectApi.Repositories.Common;

namespace PersonalProjectApi.Repositories.CardsRepositories
{
    public class SQLCardsRepository : Repository<Card>, ICardsRepository
    {
        public SQLCardsRepository(TaskBoardDbContext dbContext) : base(dbContext) { }

        public async Task<Card> CreateCardAsync(Card card)
        {
            await _dbSet.AddAsync(card);
            await _context.SaveChangesAsync();
            return card;
        }

        public async Task<List<Card>> GetAllCardsAsync(Guid userId)
        {
            var cardsDomain = await _dbSet
                .Where(c => c.BoardList.Board.BoardMembers.Any(m => m.UserId == userId) ||
                    c.BoardList.Board.OwnerId == userId)
                .Include(c => c.BoardList)
                .OrderBy(c => c.DueDate)
                .ToListAsync();
            return cardsDomain;
        }

        public async Task<Card> GetCardWithBoardMembersAsync(Guid cardId)
        {
            var cardWithMembers = await _context
                .Cards
                .Include(l => l.BoardList)
                    .ThenInclude(b => b.Board)
                        .ThenInclude(m => m.BoardMembers)
                .FirstOrDefaultAsync(c => c.Id == cardId);

            if (cardWithMembers == null)
                throw new KeyNotFoundException();

            return cardWithMembers;
        }

        public async Task<Card> UpdateCardAsync(Guid Id, Card card)
        {
            var cardDomain = await _dbSet.FirstOrDefaultAsync(c => c.Id == Id);

            if (cardDomain == null)
            {
                throw new KeyNotFoundException($"Card with id {Id} was not found.");
            }

            cardDomain.Title = card.Title;
            cardDomain.Description = card.Description;
            cardDomain.DueDate = card.DueDate;
            cardDomain.Priority = card.Priority;
            cardDomain.BoardListId = card.BoardListId;

            await _context.SaveChangesAsync();

            return cardDomain;
        }

        public async Task<Boolean> DeleteCardAsync(Guid id)
        {
            var existingCard = await _dbSet
                .Include(c => c.BoardList)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (existingCard == null)
            {
                return false;
            }

            _dbSet.Remove(existingCard);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
