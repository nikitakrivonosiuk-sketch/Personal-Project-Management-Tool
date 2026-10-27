using PersonalProjectApi.Models.Domain;
using PersonalProjectApi.Models.DTO.BoardListsDto;
using PersonalProjectApi.Repositories.ListRepositories;

namespace PersonalProjectApi.Services
{
    public class ListService
    {
        private readonly IListsRepository _listsRepository;
        public ListService(IListsRepository listsRepository)
        {
            _listsRepository = listsRepository;
        }

        public async Task<List<BoardListDto>> GetListsAsync()
        {
            var listsDomain = await _listsRepository.GetAllBoardListsAsync();

            var listsDto = listsDomain.Select(list => new BoardListDto
            {
                Id = list.Id,
                Title = list.Title,
                Position = list.Position,
            }).ToList();

            return listsDto;
        }

        public async Task<BoardListDto> CreateListAsync(CreateListRequestDto requestDto)
        {
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

        public async Task<BoardListDto> UpdateListASync(Guid id, BoardListDto  boardListDto)
        {
            var listDomain = new BoardList
            {
                Title = boardListDto.Title,
                Position = boardListDto.Position,
            };

            listDomain = await _listsRepository.UpdateListAsync(id, listDomain);

            if (listDomain == null)
            {
                throw new KeyNotFoundException($"List with id {id} was not found.");
            }

            var listDto = new BoardListDto
            {
                Id = listDomain.Id,
                Title = listDomain.Title,
                Position = listDomain.Position,
            };

            return listDto;
        }

        public async Task<string> DeleteListAsync(Guid id)
        {
            var isDeleted = await _listsRepository.DeleteListAsync(id);

            if (!isDeleted)
            {
                throw new KeyNotFoundException($"List with id {id} was not deleted. Invalid Id.");
            }

            return "Deleted succesfully";
        }
    }
}
