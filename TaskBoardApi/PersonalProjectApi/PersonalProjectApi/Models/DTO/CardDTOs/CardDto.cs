namespace PersonalProjectApi.Models.DTO.CardDTOs
{
    public class CardDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime? DueDate { get; set; }
        public string? Description { get; set; }
        public Guid PriorityId { get; set; }
        public Guid BoardListId { get; set; }

        public string BoardTitle { get; set; } = string.Empty;
        public string Priority { get; set; } = string.Empty;
    }
}
