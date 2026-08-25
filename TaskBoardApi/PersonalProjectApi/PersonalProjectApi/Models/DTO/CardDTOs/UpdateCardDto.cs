using PersonalProjectApi.Models.Domain;

namespace PersonalProjectApi.Models.DTO.CardDTOs
{
    public class UpdateCardDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime? DueDate { get; set; }
        public string? Description { get; set; }
        public Guid BoardListId { get; set; }

        public TaskPriority Priority { get; set; }
    }
}
