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
            var listsDomain = await dbContext.BoardLists.ToListAsync();

            return listsDomain;
        }
    }
}
