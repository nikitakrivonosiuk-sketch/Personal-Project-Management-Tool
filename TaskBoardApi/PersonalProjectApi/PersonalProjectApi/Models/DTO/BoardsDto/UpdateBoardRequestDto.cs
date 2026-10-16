using PersonalProjectApi.Models.DTO.BoardListsDto;
using PersonalProjectApi.Models.DTO.CardDTOs;

namespace PersonalProjectApi.Models.DTO.BoardsDto
{
    public class UpdateBoardRequestDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
    }
}
