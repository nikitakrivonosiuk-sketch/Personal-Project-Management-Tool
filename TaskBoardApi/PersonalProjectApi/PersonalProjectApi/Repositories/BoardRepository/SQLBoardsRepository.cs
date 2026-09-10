using Microsoft.EntityFrameworkCore;
using PersonalProjectApi.Data;
using PersonalProjectApi.Models.Domain;

namespace PersonalProjectApi.Repositories.BoardRepository
{
    public class SQLBoardsRepository : IBoardRepository
    {
        private readonly TaskBoardDbContext dbContext;
        public SQLBoardsRepository(TaskBoardDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<Board>> GetAllBoardsAsync()
        {
            var boards = await dbContext.Boards
                .OrderBy(b => b.CreatedAt)
                .ToListAsync();

            return boards;
        }

        public async Task<Board> GetBoardAsync(Guid id)
        {
            var board = await dbContext.Boards
                .Include(l => l.BoardLists)
                    .ThenInclude(c => c.Cards)
                .FirstOrDefaultAsync(b => b.Id == id);

            return board;
        }

        public async Task<Board> CreateBoardAsync(Board board)
        {
            await dbContext.Boards.AddAsync(board);

            await dbContext.ActivityLogs.AddAsync(new ActivityLog
            {
                Description = $"Board '{board.Title}' was created.",
                ActionType = "BoardCreated",
                BoardId = board.Id,
            });

            await dbContext.SaveChangesAsync();

            return board;
        }

        public async Task<Board> UpdateBoardAsync(Board board)
        {
            var boardDomain = await dbContext.Boards.FirstOrDefaultAsync(b => board.Id == b.Id);

            if (boardDomain == null)
            {
                return null;
            }
                
            boardDomain.Title = board.Title;

            await dbContext.ActivityLogs.AddAsync(new ActivityLog
            {
                Description = $"Board title changed from {board.Title} to {boardDomain.Title} was created.",
                ActionType = "BoardCreated",
                BoardId = board.Id,
            });

            await dbContext.SaveChangesAsync();
            return boardDomain;
        }

        public async Task<Boolean> DeleteBoardAsync(Guid id)
        {
            var boardDomain = await dbContext.Boards
                .Include(b => b.BoardLists)
                .ThenInclude(l => l.Cards)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (boardDomain == null)
            {
                return false;
            }

            var listIds = boardDomain.BoardLists.Select(l => l.Id).ToList();
            var cardIds = boardDomain.BoardLists.SelectMany(l => l.Cards).Select(c => c.Id).ToList();

            if (cardIds.Any())
            {
                await dbContext.ActivityLogs
                    .Where(log => log.CardId != null && cardIds.Contains(log.CardId.Value))
                    .ExecuteUpdateAsync(s => s.SetProperty(l => l.CardId, (Guid?)null));
            }

            if (listIds.Any())
            {
                await dbContext.ActivityLogs
                    .Where(log => log.BoardListId != null && listIds.Contains(log.BoardListId.Value))
                    .ExecuteUpdateAsync(s => s.SetProperty(l => l.BoardListId, (Guid?)null));
            }

            var board = await dbContext.Boards.FindAsync(id);

            await dbContext.ActivityLogs.AddAsync(new ActivityLog
            {
                Description = $"Board {board.Id} was deleted.",
                ActionType = "BoardDeleted",
                BoardId = board.Id,
            });

            dbContext.Boards.Remove(boardDomain);
            await dbContext.SaveChangesAsync();
            return true;
        }
    }
}
