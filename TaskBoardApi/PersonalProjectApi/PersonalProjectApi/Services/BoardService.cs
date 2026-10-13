using PersonalProjectApi.Models.Domain;
using PersonalProjectApi.Models.DTO.BoardListsDto;
using PersonalProjectApi.Models.DTO.BoardsDto;
using PersonalProjectApi.Models.DTO.CardDTOs;
using PersonalProjectApi.Repositories.BoardRepository;

namespace PersonalProjectApi.Services
{
    public class BoardService
    {
        private readonly IBoardRepository _boardRepository;

        public BoardService(IBoardRepository boardRepository)
        {
            _boardRepository = boardRepository;
        }

        public async Task<List<Board>> GetAllBoards()
        {
            var boardsDomain = await _boardRepository.GetAllBoardsAsync();

            var boardsDto = boardsDomain.Select(b => new Board
            {
                Id = b.Id,
                Title = b.Title,
                CreatedAt = b.CreatedAt,
            }).ToList();

            return boardsDto;
        }

        public async Task<BoardDto> GetBoardByIdAsync(Guid id)
        {
            var boardDomain = await _boardRepository.GetBoardAsync(id);

            if (boardDomain == null)
            {
                throw new KeyNotFoundException($"Board with id {id} was not found.");
            }

            var boardDto = new BoardDto
            {
                Id = id,
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

        public async Task<BoardDto> CreateBoardAsync(CreatingBoardDto creatingRequestDto)
        {
            var boardDomain = new Board
            {
                Title = creatingRequestDto.Title,
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

        public async Task<BoardDto> UpdateBoardAsync(UpdateBoardRequestDto requestDto)
        {
            var boardDomain = new Board
            {
                Id = requestDto.Id,
                Title = requestDto.Title,
            };

            boardDomain = await _boardRepository.UpdateBoardAsync(boardDomain);

            if (boardDomain == null)
            {
                throw new KeyNotFoundException($"Board with id {requestDto.Id} was not found.");
            }

            var boardDto = new BoardDto
            {
                Id = boardDomain.Id,
                Title = boardDomain.Title,
            };

            return boardDto;
        }

        public async Task<string> DeleteBoardAsync(Guid id)
        {
            var isDeleted = await _boardRepository.DeleteBoardAsync(id);

            if (!isDeleted)
            {
                throw new KeyNotFoundException($"Board with id {id} was not found.");
            }

            return "Deleted succesfully";
        }
    }
}
