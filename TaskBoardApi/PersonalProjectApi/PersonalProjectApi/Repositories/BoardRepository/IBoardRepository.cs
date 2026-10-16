using PersonalProjectApi.Models.Domain;
using PersonalProjectApi.Repositories.Common;

namespace PersonalProjectApi.Repositories.BoardRepository
{
        public interface IBoardRepository : IRepository<Board>
        {
            Task<List<Board>> GetAllBoardsAsync(int pageNumber, int pageSize);
            Task<Board?> GetBoardAsync(Guid id);
            Task<Board> CreateBoardAsync(Board board);
            Task<Board> UpdateBoardAsync(Board board);
            Task<bool> DeleteBoardAsync(Guid id);
        }
}
