using PersonalProjectApi.Models.DTO.BoardListsDto;

namespace PersonalProjectApi.Models.DTO.BoardsDto
{
    public class BoardDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public List<BoardListDto> BoardLists { get; set; }
    }
}
