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

            await dbContext.SaveChangesAsync();
            return boardDomain;
        }

        public async Task<Boolean> DeleteBoardAsync(Guid id)
        {
            var boardDomain = await dbContext.Boards.FindAsync(id);

            if (boardDomain == null)
            {
                return false;
            }

            dbContext.Boards.Remove(boardDomain);
            await dbContext.SaveChangesAsync();
            return true;
        }
    }
}
