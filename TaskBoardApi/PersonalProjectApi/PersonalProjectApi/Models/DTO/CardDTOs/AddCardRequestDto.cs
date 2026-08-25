using PersonalProjectApi.Models.Domain;

namespace PersonalProjectApi.Models.DTO.CardDTOs
{
    public class AddCardRequestDto
    {
        public string Title { get; set; }
        public DateTime? DueDate { get; set; }
        public string? Description { get; set; }
        public Guid BoardListId { get; set; }
        public TaskPriority Priority { get; set; }
    }
}
