using PersonalProjectApi.ExceptionHandler.Exceptions;
using PersonalProjectApi.Models.Domain;
using PersonalProjectApi.Models.DTO.BoardListsDto;
using PersonalProjectApi.Models.DTO.BoardsDto;
using PersonalProjectApi.Models.DTO.CardDTOs;
using PersonalProjectApi.Models.Enums;
using PersonalProjectApi.Repositories.BoardRepository;
using PersonalProjectApi.Services.Authorization;

namespace PersonalProjectApi.Services
{
    public class BoardService
    {
        private readonly IBoardRepository _boardRepository;
        private readonly BoardAuthorizationService _authService;

        public BoardService(IBoardRepository boardRepository, BoardAuthorizationService boardAuthorizationService)
        {
            _boardRepository = boardRepository;
            _authService = boardAuthorizationService;
        }

        public async Task<List<BoardDto>> GetAllBoards(Guid userId, int pageNumber, int pageSize)
        {
            var boardsDomain = await _boardRepository.GetAllBoardsAsync(userId, pageNumber, pageSize);

            var boardsDto = boardsDomain.Select(b => new BoardDto
            {
                Id = b.Id,
                Title = b.Title,
                CreatedAt = b.CreatedAt,
            }).ToList();

            return boardsDto;
        }

        public async Task<BoardDto> GetBoardByIdAsync(Guid userId, Guid boardId)
        {
            var boardDomain = await _boardRepository.GetBoardAsync(boardId);

            if (boardDomain == null)
            {
                throw new KeyNotFoundException($"Board with id {boardId} was not found.");
            }

            _authService.EnsureCanRead(userId, boardDomain);

            var boardDto = new BoardDto
            {
                Id = boardId,
                Title = boardDomain.Title,
                CreatedAt = boardDomain.CreatedAt,
                BoardLists = boardDomain.BoardLists.Select(l => new BoardListDto
                {
                    Id = l.Id,
                    Title = l.Title,
                    Position = l.Position,
                    Cards = l.Cards.Select(c => new CardDto
                    {
                        Id = c.Id,
                        Title = c.Title,
                        Priority = c.Priority,
                        Description = c.Description,
                        DueDate = c.DueDate,
                        BoardListId = l.Id,
                    }).ToList()
                }).ToList()
            };

            return boardDto;
        }

        public async Task<BoardDto> CreateBoardAsync(Guid userId, CreatingBoardDto creatingRequestDto)
        {
            var boardDomain = new Board
            {
                Title = creatingRequestDto.Title,
                OwnerId = userId,
            };

            boardDomain = await _boardRepository.CreateBoardAsync(boardDomain);

            var boardDto = new BoardDto
            {
                Id = boardDomain.Id,
                Title = boardDomain.Title,
                CreatedAt = boardDomain.CreatedAt,
            };

            return boardDto;
        }

        public async Task<BoardDto> UpdateBoardAsync(Guid boardId, UpdateBoardRequestDto requestDto, Guid userId)
        {
            var boardWithMembers = await _boardRepository.GetBoardWithMembersAsync(boardId);

            if (boardWithMembers == null)
            {
                throw new KeyNotFoundException($"Board with id {boardId} was not found.");
            }

            _authService.EnsureCanEdit(userId, boardWithMembers);

            boardWithMembers.Title = requestDto.Title;

            var boardDomain = await _boardRepository.UpdateBoardAsync(boardWithMembers);

            var boardDto = new BoardDto
            {
                Id = boardDomain.Id,
                Title = boardDomain.Title,
            };

            return boardDto;
        }

        public async Task<string> DeleteBoardAsync(Guid boardId, Guid userId)
        {
            var board = await _boardRepository.GetBoardWithMembersAsync(boardId);

            if (board == null)
                throw new KeyNotFoundException($"Board with id {boardId} was not found.");

            _authService.EnsureCanDelete(userId, board);

            await _boardRepository.DeleteBoardAsync(boardId);

            return "Deleted succesfully";
        }
    }
}
