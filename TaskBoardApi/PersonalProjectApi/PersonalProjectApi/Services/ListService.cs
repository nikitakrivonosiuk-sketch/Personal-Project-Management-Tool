using PersonalProjectApi.Models.Domain;
using PersonalProjectApi.Models.DTO.BoardListsDto;
using PersonalProjectApi.Repositories.BoardRepository;
using PersonalProjectApi.Repositories.ListRepositories;
using PersonalProjectApi.Services.Authorization;

namespace PersonalProjectApi.Services
{
    public class ListService
    {
        private readonly IListsRepository _listsRepository;
        private readonly BoardAuthorizationService _authService;
        private readonly IBoardRepository _boardRepository;
        public ListService(IListsRepository listsRepository, 
            BoardAuthorizationService boardAuthorizationService, 
            IBoardRepository boardRepository)
        {
            _listsRepository = listsRepository;
            _authService = boardAuthorizationService;
            _boardRepository = boardRepository;
        }

        public async Task<List<BoardListDto>> GetListsAsync(Guid userId)
        {
            var listsDomain = await _listsRepository.GetAllBoardListsAsync(userId);

            var listsDto = listsDomain.Select(list => new BoardListDto
            {
                Id = list.Id,
                Title = list.Title,
                Position = list.Position,
            }).ToList();

            return listsDto;
        }

        public async Task<BoardListDto> CreateListAsync(Guid userId, CreateListRequestDto requestDto)
        {
            var board = await _boardRepository.GetBoardWithMembersAsync(requestDto.BoardId);

            _authService.EnsureCanEdit(userId, board);

            var listDomain = new BoardList
            {
                Title = requestDto.Title,
                BoardId = requestDto.BoardId,
            };

            listDomain = await _listsRepository.CreateListAsync(listDomain);

            var listDto = new BoardListDto
            {
                Id = listDomain.Id,
                Title = listDomain.Title,
            };

            return listDto;
        }

        public async Task<BoardListDto> UpdateListASync(Guid listId, BoardListDto  boardListDto, Guid userId)
        {
            var list = await _listsRepository.GetListWithBoardMembersAsync(listId);

            _authService.EnsureCanEdit(userId, list.Board);

            var listDomain = new BoardList
            {
                Title = boardListDto.Title,
                Position = boardListDto.Position,
            };

            listDomain = await _listsRepository.UpdateListAsync(listId, listDomain);

            if (listDomain == null)
            {
                throw new KeyNotFoundException($"List with id {listId} was not found.");
            }

            var listDto = new BoardListDto
            {
                Id = listDomain.Id,
                Title = listDomain.Title,
                Position = listDomain.Position,
            };

            return listDto;
        }

        public async Task<string> DeleteListAsync(Guid listId, Guid userId)
        {
            var listWithMembers = await _listsRepository.GetListWithBoardMembersAsync(listId); 

            _authService.EnsureCanEdit(userId, listWithMembers.Board);

            var isDeleted = await _listsRepository.DeleteListAsync(listId);

            if (!isDeleted)
            {
                throw new KeyNotFoundException($"List with id {listId} was not deleted. Invalid Id.");
            }

            return "Deleted succesfully";
        }
    }
}
