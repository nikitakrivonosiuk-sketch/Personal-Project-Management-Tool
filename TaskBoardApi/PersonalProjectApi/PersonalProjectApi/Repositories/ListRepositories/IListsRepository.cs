using PersonalProjectApi.Models.Domain;

namespace PersonalProjectApi.Repositories.ListRepositories
{
    public interface IListsRepository
    {
        Task<List<BoardList>> GetAllBoardListsAsync();
    }
}
