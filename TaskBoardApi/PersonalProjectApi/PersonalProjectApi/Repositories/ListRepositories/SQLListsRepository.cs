using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using PersonalProjectApi.Data;
using PersonalProjectApi.Models.Domain;

namespace PersonalProjectApi.Repositories.ListRepositories
{
    public class SQLListsRepository : IListsRepository
    {
        private readonly TaskBoardDbContext dbContext;
        public SQLListsRepository(TaskBoardDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<BoardList>> GetAllBoardListsAsync()
        {
            var listsDomain = await dbContext.BoardLists.OrderBy(l => l.Position).ToListAsync();

            return listsDomain;
        }

        public async Task<BoardList> CreateListAsync(BoardList boardList)
        {
            var maxPosittion = await dbContext.BoardLists.MaxAsync(list => (int?)list.Position) ?? 0;
            boardList.Position = maxPosittion + 1;

            await dbContext.BoardLists.AddAsync(boardList);

            await dbContext.ActivityLogs.AddAsync(new ActivityLog
            {
                BoardListId = boardList.Id,
                Description = $"'{boardList.Title}' was created.",
                CreatedAt = DateTime.Now,
                ActionType = "Created"
            });

            await dbContext.SaveChangesAsync();

            return boardList;
        }

        public async Task<BoardList> UpdateListAsync(Guid id, BoardList boardList)
        {
            var listDomain = await dbContext.BoardLists.FirstOrDefaultAsync(l => l.Id == id);

            if (listDomain == null)
            {
                return null;
            }

            // Title changed
            if (boardList.Title != listDomain.Title)
            {
                await dbContext.ActivityLogs.AddAsync(new ActivityLog
                {
                    BoardListId = listDomain.Id,
                    Description = $"List tittle was changed from '{listDomain.Title}' to '{boardList.Title}'",
                    ActionType = "TitleChanged",
                });
            }

            listDomain.Title = boardList.Title;
            listDomain.Position = boardList.Position;

            await dbContext.SaveChangesAsync();

            return listDomain;
        }

        public async Task<Boolean> DeleteListAsync(Guid id)
        {
            var deletedList = await dbContext.BoardLists.FindAsync(id);

            if (deletedList == null)
            {
                return false;
            }

            var cardIds = await dbContext.Cards
                .Where(c => c.BoardListId == id)
                .Select(c => c.Id)
                .ToListAsync();

            if (cardIds.Any())
            {
                await dbContext.ActivityLogs
                    .Where(log => log.CardId != null && cardIds.Contains(log.CardId.Value))
                    .ExecuteUpdateAsync(s => s.SetProperty(l => l.CardId, (Guid?)null));
            }

            await dbContext.ActivityLogs
                .Where(log => log.BoardListId == id)
                .ExecuteUpdateAsync(s => s.SetProperty(l => l.BoardListId, (Guid?)null));

            await dbContext.ActivityLogs.AddAsync(new ActivityLog
            {
                BoardListId = null,
                Description = $"You deleted '{deletedList.Title}' list.",
                ActionType = "Deleted",
            });

            dbContext.BoardLists.Remove(deletedList);
            await dbContext.SaveChangesAsync();

            return true;
        }
    }
}
