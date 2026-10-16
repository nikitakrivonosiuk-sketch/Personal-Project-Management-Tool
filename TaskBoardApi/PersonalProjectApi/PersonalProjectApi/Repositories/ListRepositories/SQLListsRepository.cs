using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using PersonalProjectApi.Data;
using PersonalProjectApi.Models.Domain;
using PersonalProjectApi.Repositories.Common;

namespace PersonalProjectApi.Repositories.ListRepositories
{
    public class SQLListsRepository : Repository<BoardList>, IListsRepository
    {
        public SQLListsRepository(TaskBoardDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<BoardList>> GetAllBoardListsAsync()
        {
            var listsDomain = await _dbSet.OrderBy(l => l.Position).ToListAsync();

            return listsDomain;
        }

        public async Task<BoardList> CreateListAsync(BoardList boardList)
        {
            var maxPosittion = await _dbSet.MaxAsync(list => (int?)list.Position) ?? 0;
            boardList.Position = maxPosittion + 1;

            await _dbSet.AddAsync(boardList);

            await _context.SaveChangesAsync();

            return boardList;
        }

        public async Task<BoardList> UpdateListAsync(Guid id, BoardList boardList)
        {
            var listDomain = await _dbSet.FirstOrDefaultAsync(l => l.Id == id);

            if (listDomain == null)
            {
                throw new KeyNotFoundException($"List with id {id} was not found.");
            }

            listDomain.Title = boardList.Title;
            listDomain.Position = boardList.Position;

            await _context.SaveChangesAsync();

            return listDomain;
        }

        public async Task<Boolean> DeleteListAsync(Guid id)
        {
            var deletedList = await _dbSet
                .Include(l =>  l.Cards)
                .FirstOrDefaultAsync(l => l.Id == id);

            if (deletedList == null)
            {
                return false;
            }

            _dbSet.Remove(deletedList);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
