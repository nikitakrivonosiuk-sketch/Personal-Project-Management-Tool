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

            var list = await dbContext.BoardLists.FindAsync(card.BoardListId);

            var log = new ActivityLog
            {
                CardId = card.Id,
                BoardId = list?.BoardId,
                ActionType = ActionType.CardCreated,
                Description = $"You created '{card.Title}' card"
            };

            await dbContext.ActivityLogs.AddAsync(log);
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
                throw new KeyNotFoundException($"Card with id {Id} was not found.");
            }

            var list = await dbContext.BoardLists.FindAsync(card.BoardListId);

            // List changed
            if (cardDomain.BoardListId != card.BoardListId)
            {
                var newList = await dbContext.BoardLists.FindAsync(cardDomain.BoardListId);
                var oldList = await dbContext.BoardLists.FindAsync(card.BoardListId);

                var moveLog = new ActivityLog
                {
                    CardId = cardDomain.Id,
                    BoardId = list?.BoardId,
                    ActionType = ActionType.CardMoved,
                    Description = $"You moved '{cardDomain.Title}' from {newList?.Title} to {oldList?.Title}",
                };

                await dbContext.ActivityLogs.AddAsync(moveLog);
            }
            // title changed
            if (cardDomain.Title != card.Title)
            {
                await dbContext.ActivityLogs.AddAsync(new ActivityLog
                {
                    CardId= cardDomain.Id,
                    ActionType = ActionType.CardTitleEdited,
                    Description = $"You renamed this card from '{card.Title}' to '{cardDomain.Title}'",
                });
            }
            // Priority changed
            if (cardDomain.Priority != card.Priority)
            {
                await dbContext.ActivityLogs.AddAsync(new ActivityLog
                {
                    CardId = cardDomain.Id,
                    ActionType = ActionType.CardPriorityEdited,
                    Description = $"You changed priority from '{cardDomain.Priority}' to '{card.Priority}'",
                });
            }
            // Date changed
            if (cardDomain.DueDate != card.DueDate)
            {
                await dbContext.ActivityLogs.AddAsync(new ActivityLog
                {
                    CardId = cardDomain.Id,
                    ActionType = ActionType.CardDueDateEdited,
                    Description = $"You changed due date from '{card.DueDate}' to '{cardDomain.DueDate}'",
                });
            }
            // Desription changed
            if (cardDomain.Description != card.Description)
            {
                await dbContext.ActivityLogs.AddAsync(new ActivityLog
                {
                    CardId = cardDomain.Id,
                    ActionType = ActionType.CardDescriptionEdited,
                    Description = $"You changed description of '{cardDomain.Title}' card",
                });
            }

            cardDomain.Title = card.Title;
            cardDomain.Description = card.Description;
            cardDomain.DueDate = card.DueDate;
            cardDomain.Priority = card.Priority;
            cardDomain.BoardListId = card.BoardListId;

            await dbContext.SaveChangesAsync();

            return cardDomain;
        }

        public async Task<Boolean> DeleteCardAsync(Guid id)
        {
            var existingCard = await dbContext.Cards.Include(b => b.BoardList).FirstOrDefaultAsync(c => c.Id == id);

            if (existingCard == null)
            {
                return false;
            }

            await dbContext.ActivityLogs
                           .Where(log => log.CardId == id)
                           .ExecuteUpdateAsync(s => s.SetProperty(l => l.CardId, (Guid?)null));

            var list = await dbContext.BoardLists.FindAsync(id);

            var deleteLog = new ActivityLog
            {
                CardId = null,
                BoardId = existingCard.BoardList?.BoardId,
                ActionType = ActionType.CardDeleted,  
                Description = $"You deleted '{existingCard.Title}'",
            };

            dbContext.Cards.Remove(existingCard);
            await dbContext.ActivityLogs.AddAsync(deleteLog);
            await dbContext.SaveChangesAsync();

            return true;
        }
    }
}
