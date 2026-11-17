using PersonalProjectApi.Models.Domain;
using PersonalProjectApi.Models.DTO.BoardListsDto;
using PersonalProjectApi.Models.Enums;

namespace PersonalProjectApi.Models.DTO.BoardsDto
{
    public class BoardDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public bool IsOwner { get; set; }
        public string OwnerEmail { get; set; } = string.Empty;  
        public UserLevelAccess? CurrentLevel { get; set; }
        public List<BoardListDto> BoardLists { get; set; } = new List<BoardListDto>();
    }
}
