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

            listDomain.Title = boardList.Title;
            listDomain.Position = boardList.Position;

            await dbContext.SaveChangesAsync();

            return listDomain;
        }

        public async Task<Boolean> DeleteListAsync(Guid id)
        {
            var deletedLists = await dbContext.BoardLists.Where(l => l.Id == id).ExecuteDeleteAsync();

            if (deletedLists == 0)
            {
                return false;
            }

            return true;
        }
    }
}
