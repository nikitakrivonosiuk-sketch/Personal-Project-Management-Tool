using PersonalProjectApi.Models.Domain;
using PersonalProjectApi.Models.DTO.CardDTOs;

namespace PersonalProjectApi.Models.DTO.BoardListsDto
{
    public class BoardListDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int Position { get; set; }
        public ICollection<CardDto> Cards { get; set; } = new List<CardDto>();
    }
}
