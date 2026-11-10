using Microsoft.EntityFrameworkCore;
using PersonalProjectApi.Data;
using PersonalProjectApi.Models.Domain;
using PersonalProjectApi.Repositories.Common;

namespace PersonalProjectApi.Repositories.BoardMembersRepositories
{
    public class SQLBoardMemberRepository : Repository<BoardMember>, IBoardMembersRepository
    {
        public SQLBoardMemberRepository(TaskBoardDbContext dbContext) : base(dbContext) { }
        public async Task<BoardMember> AddBoardMemberAsync(BoardMember boardMember)
        {
            await _dbSet.AddAsync(boardMember);
            await _context.SaveChangesAsync();
            return boardMember;
        }
        public async Task<BoardMember> UpdateBoardMemberAsync(BoardMember boardMember)
        {
            var foundedMember = await _context.BoardMembers.FirstOrDefaultAsync(
                m => m.UserId == boardMember.UserId &&
                m.BoardId == boardMember.BoardId);

            if (foundedMember == null)
                throw new KeyNotFoundException("Board member was not found.");

            foundedMember.UserLevel = boardMember.UserLevel;

            await _context.SaveChangesAsync();
            return foundedMember;
        }
        public async Task<BoardMember> DeleteBoardMemberAsync(Guid targetId, Guid boardId)
        {
            var boardMember = await _dbSet.FirstOrDefaultAsync(m => m.UserId == targetId && m.BoardId == boardId);

            if (boardMember == null)
                throw new KeyNotFoundException("User wasn't found.");

            _dbSet.Remove(boardMember);
            await _context.SaveChangesAsync();
            return boardMember;
        }
    }
}
