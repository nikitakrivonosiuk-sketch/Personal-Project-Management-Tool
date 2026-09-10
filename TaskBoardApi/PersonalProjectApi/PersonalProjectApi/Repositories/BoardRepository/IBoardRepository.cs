using PersonalProjectApi.Models.Domain;

namespace PersonalProjectApi.Repositories.BoardRepository
{
    public interface IBoardRepository
    {
        Task<List<Board>> GetAllBoardsAsync();
        Task<Board> GetBoardAsync(Guid id);
        Task<Board> CreateBoardAsync(Board board);
        Task<Board> UpdateBoardAsync(Board board);
        Task<Boolean> DeleteBoardAsync(Guid id);
    }
}
