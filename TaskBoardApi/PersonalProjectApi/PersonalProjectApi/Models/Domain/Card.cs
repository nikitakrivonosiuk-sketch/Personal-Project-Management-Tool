namespace PersonalProjectApi.Models.Domain
{
    public class Card
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public DateTime? DueDate { get; set; }
        public string? Description { get; set; }
        public Guid PriorityId { get; set; }
        public Guid BoardListId { get; set; }

        // Navigation Properties
        public Priority Priority { get; set; }
        public BoardList BoardList { get; set; }
    }
}
