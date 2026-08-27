using PersonalProjectApi.Models.Domain;

namespace PersonalProjectApi.Repositories.ListRepositories
{
    public interface IListsRepository
    {
        Task<List<BoardList>> GetAllBoardListsAsync();
        Task<BoardList> CreateListAsync(BoardList boardList);
        Task<BoardList> UpdateListAsync(Guid id, BoardList boardList);
        Task<Boolean> DeleteListAsync(Guid id);
    }
}
