using PersonalProjectApi.Models.Domain;
using PersonalProjectApi.Repositories.Common;

namespace PersonalProjectApi.Repositories.BoardMembersRepositories
{
    public interface IBoardMembersRepository : IRepository<BoardMember>
    {
        Task<BoardMember> AddBoardMemberAsync(BoardMember boardMember);
        Task<BoardMember> UpdateBoardMemberAsync(BoardMember boardMember);
        Task<BoardMember> DeleteBoardMemberAsync(Guid targetId, Guid boardId);
    }
}
