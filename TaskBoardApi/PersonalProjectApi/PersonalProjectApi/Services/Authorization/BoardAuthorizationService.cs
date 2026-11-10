using PersonalProjectApi.ExceptionHandler.Exceptions;
using PersonalProjectApi.Models.Domain;
using PersonalProjectApi.Models.Enums;

namespace PersonalProjectApi.Services.Authorization
{
    public class BoardAuthorizationService
    {
        public void EnsureCanRead(Guid userId, Board board)
        {
            if (board.OwnerId == userId)
                return;

            if (!board.BoardMembers.Any(m => m.UserId == userId))
                throw new KeyNotFoundException();
        }
        public void EnsureCanEdit(Guid userId, Board board)
        {
            if (board.OwnerId == userId)
                return;

            var member = board.BoardMembers.FirstOrDefault(x => x.UserId == userId);

            if (member == null)
                throw new KeyNotFoundException();

            if (member.UserLevel is not (UserLevelAccess.Editor or UserLevelAccess.Admin))
                throw new ForbiddenException();
        }
        public void EnsureCanDelete(Guid userId, Board board)
        {
            if (board.OwnerId == userId)
                return;
            
            if (!board.BoardMembers.Any(x => x.UserId == userId))
                throw new KeyNotFoundException();

            throw new ForbiddenException();
        }
        public void EnsureCanAddMembers(Guid userId, Board board, UserLevelAccess levelAccess)
        {
            if (board.OwnerId == userId)
            {
                return;
            }

            var member = board.BoardMembers.FirstOrDefault(x => x.UserId == userId);

            if (member == null)
            {
                throw new KeyNotFoundException();
            }

            if (member.UserLevel is not UserLevelAccess.Admin || levelAccess == UserLevelAccess.Admin)
            {
                throw new ForbiddenException();
            }
        }
        public void EnsureCanChangeRoles(Guid userId, Board board, BoardMember targetMember, UserLevelAccess levelAccess)
        {
            if (board.OwnerId == userId)
                return;

            var member = board.BoardMembers.FirstOrDefault(x => x.UserId == userId);

            if (member == null)
                throw new KeyNotFoundException();

            if (member.UserLevel is not UserLevelAccess.Admin)
                throw new ForbiddenException();

            if (targetMember.UserLevel == UserLevelAccess.Admin || levelAccess == UserLevelAccess.Admin)
                throw new ForbiddenException();
        }
        public void EnsureCanDeleteBoardMembers(Guid userId, Board board, BoardMember targetMember)
        {
            if (board.OwnerId == userId)
                return;

            var member = board.BoardMembers.FirstOrDefault(x => x.UserId == userId);
            if (member == null)
                throw new KeyNotFoundException();

            if (member.UserLevel != UserLevelAccess.Admin || targetMember.UserLevel == UserLevelAccess.Admin)
                throw new ForbiddenException(); 
        }
    }
}
