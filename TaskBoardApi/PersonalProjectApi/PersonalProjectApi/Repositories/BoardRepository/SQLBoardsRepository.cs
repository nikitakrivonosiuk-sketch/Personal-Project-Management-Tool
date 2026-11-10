using Microsoft.EntityFrameworkCore;
using PersonalProjectApi.Data;
using PersonalProjectApi.Models.Domain;
using PersonalProjectApi.Repositories.Common;

namespace PersonalProjectApi.Repositories.BoardRepository
{
    public class SQLBoardsRepository : Repository<Board>, IBoardRepository
    {
        public SQLBoardsRepository(TaskBoardDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<Board>> GetAllBoardsAsync(Guid userId, int pageNumber, int pageSize)
        {
            var boards = await _dbSet
                .Where(b => b.OwnerId == userId || b.BoardMembers.Any(m => m.UserId == userId))
                .OrderBy(b => b.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return boards;
        }

        public async Task<Board?> GetBoardAsync(Guid boardId)
        {
            var board = await _dbSet
                .Include(m =>  m.BoardMembers)
                .Include(l => l.BoardLists)
                    .ThenInclude(c => c.Cards)
                .FirstOrDefaultAsync(b => b.Id == boardId);

            return board;
        }

        public async Task<Board> CreateBoardAsync(Board board)
        {
            await _dbSet.AddAsync(board);

            await _context.SaveChangesAsync();

            return board;
        }

        public async Task<Board> GetBoardWithMembersAsync(Guid boardId)
        {
            var board = await _context.Boards
                .Include(m => m.BoardMembers)
                    .ThenInclude(u => u.User)
                .FirstOrDefaultAsync(b => b.Id == boardId);

            if (board == null)
                throw new KeyNotFoundException();

            return board;
        }

        public async Task<Board> UpdateBoardAsync(Board board)
        {
            var boardDomain = await _dbSet.FirstOrDefaultAsync(b => board.Id == b.Id);

            if (boardDomain == null)
            {
                throw new KeyNotFoundException($"Board with id {board.Id} was not found.");
            }

            boardDomain.Title = board.Title;

            await _context.SaveChangesAsync();
            return boardDomain;
        }

        public async Task<Boolean> DeleteBoardAsync(Guid id)
        {
            var boardDomain = await _dbSet
                .Include(b => b.BoardLists)
                    .ThenInclude(l => l.Cards)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (boardDomain == null)
            {
                return false;
            }

            _dbSet.Remove(boardDomain);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
