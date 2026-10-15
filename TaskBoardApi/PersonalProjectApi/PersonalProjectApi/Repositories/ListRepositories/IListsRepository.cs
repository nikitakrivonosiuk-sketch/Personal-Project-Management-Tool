using PersonalProjectApi.Models.Domain;
using PersonalProjectApi.Repositories.Common;

namespace PersonalProjectApi.Repositories.ListRepositories
{
    public interface IListsRepository : IRepository<BoardList>
    {
        Task<List<BoardList>> GetAllBoardListsAsync();
        Task<BoardList> CreateListAsync(BoardList boardList);
        Task<BoardList> UpdateListAsync(Guid id, BoardList boardList);
        Task<Boolean> DeleteListAsync(Guid id);
    }
}
