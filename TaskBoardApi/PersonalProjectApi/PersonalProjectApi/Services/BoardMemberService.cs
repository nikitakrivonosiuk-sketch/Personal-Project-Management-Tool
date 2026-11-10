using Microsoft.AspNetCore.Identity;
using PersonalProjectApi.ExceptionHandler.Exceptions;
using PersonalProjectApi.Models.Domain;
using PersonalProjectApi.Models.DTO.UserDTOs;
using PersonalProjectApi.Repositories.BoardMembersRepositories;
using PersonalProjectApi.Repositories.BoardRepository;
using PersonalProjectApi.Services.Authorization;

namespace PersonalProjectApi.Services
{
    public class BoardMemberService
    {
        private readonly IBoardRepository _boardRepository;
        private readonly IBoardMembersRepository _membersRepository;
        private readonly BoardAuthorizationService _authService;
        private readonly UserManager<AppUser> _userManager;
        public BoardMemberService(IBoardRepository boardRepository,
            BoardAuthorizationService boardAuthorizationService,
            UserManager<AppUser> userManager,
            IBoardMembersRepository boardMembersRepository)
        {
            _boardRepository = boardRepository;
            _authService = boardAuthorizationService;
            _userManager = userManager;
            _membersRepository = boardMembersRepository;
        }

        public async Task<List<BoardMemberDto>> GetAllBoardUsersAsync(Guid boardId, Guid userId)
        {
            var boardWithMembers = await _boardRepository.GetBoardWithMembersAsync(boardId);

            if (boardWithMembers == null)
                throw new KeyNotFoundException("Board was not found");

            _authService.EnsureCanRead(userId, boardWithMembers);

            var members = boardWithMembers.BoardMembers
                .Select(member => new BoardMemberDto
                {
                    UserId = member.UserId,
                    Email = member.User.Email ?? string.Empty,
                    UserLevel = member.UserLevel,
                }).ToList();

            return members;
        }
        public async Task<BoardMemberDto> AddUserToBoard(AddBoardMemberRequestDto requestDto, Guid boardId, Guid userId)
        {
            var board = await _boardRepository.GetBoardWithMembersAsync(boardId);

            if (board == null)
                throw new KeyNotFoundException("No board was found");

            _authService.EnsureCanAddMembers(userId, board, requestDto.UserLevel);

            var user = await _userManager.FindByEmailAsync(requestDto.Email);

            if (user == null) 
                throw new KeyNotFoundException("User was not found.");

            if (user.Id == board.OwnerId) 
                throw new ConflictException("Owner of board cannot be in other role.");

            if (board.BoardMembers.Any(m => m.UserId == user.Id)) 
                throw new ConflictException("User is already a board member.");

            var boardMember = new BoardMember
            {
                BoardId = boardId,
                UserId = user.Id,
                UserLevel = requestDto.UserLevel
            };

            var result = await _membersRepository.AddBoardMemberAsync(boardMember);

            var memberDto = new BoardMemberDto
            {
                UserId = user.Id,
                Email = user.Email ?? string.Empty,
                UserLevel = requestDto.UserLevel,
            };

            return memberDto;
        }
        public async Task<BoardMemberDto> UpdateBoardMemberAsync(Guid boardId, Guid memberUserId, Guid userId, UpdateBoardMemberRequestDto requestDto)
        {
            var boardWithMembers = await _boardRepository.GetBoardWithMembersAsync(boardId);

            if (boardWithMembers == null)
                throw new KeyNotFoundException("Board or member was not found.");

            if (boardWithMembers.OwnerId == memberUserId)
                throw new ConflictException("The board owner cannot be managed as a member.");

            var targerMember = boardWithMembers.BoardMembers.FirstOrDefault(m => m.UserId == memberUserId);
            if (targerMember == null)
                throw new KeyNotFoundException("Board member was not found.");


            _authService.EnsureCanChangeRoles(userId, boardWithMembers, targerMember, requestDto.LevelAccess);

            var user = await _userManager.FindByIdAsync(memberUserId.ToString());

            if (user == null)
                throw new KeyNotFoundException("User was not found.");

            if (user.Id == boardWithMembers.OwnerId)
                throw new ConflictException("Owner of board cannot be in other role.");

            var boardMember = new BoardMember
            {
                BoardId = boardId,
                UserId = user.Id,
                UserLevel = requestDto.LevelAccess
            };

            var result = await _membersRepository.UpdateBoardMemberAsync(boardMember);

            var boardMemberDto = new BoardMemberDto
            {
                Email = user.Email ?? string.Empty,
                UserId = user.Id,
                UserLevel = result.UserLevel,
            };

            return boardMemberDto;
        }
        public async Task<string> DeleteBoardMemberAsync(Guid boardId, Guid memberUserId, Guid userId)
        {
            var boardWithMembers = await _boardRepository.GetBoardWithMembersAsync(boardId);

            if (boardWithMembers == null)
                throw new KeyNotFoundException("Board was not found.");

            var targetMember = boardWithMembers.BoardMembers.FirstOrDefault(m => m.UserId == memberUserId);
            if (targetMember == null)
                throw new KeyNotFoundException();

            _authService.EnsureCanDeleteBoardMembers(userId, boardWithMembers, targetMember);

            var result = await _membersRepository.DeleteBoardMemberAsync(targetMember.UserId, boardId);

            return "Board member deleted successfully";
        }
    }
}
