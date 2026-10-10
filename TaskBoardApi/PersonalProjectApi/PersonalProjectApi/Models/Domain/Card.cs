namespace PersonalProjectApi.Models.Domain
{
    public class Card
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public DateTime? DueDate { get; set; }
        public string? Description { get; set; }
        public Guid BoardListId { get; set; }
        public Priority Priority { get; set; }

        // Navigation Properties
        public BoardList BoardList { get; set; }
    }
}
