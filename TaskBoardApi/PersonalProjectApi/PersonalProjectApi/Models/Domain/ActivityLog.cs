namespace PersonalProjectApi.Models.Domain
{
    public class ActivityLog
    {
        public Guid Id { get; set; }
        public Guid? CardId { get; set; }
        public Guid? BoardListId { get; set; }
        public string Description { get; set; } = string.Empty;
        public string ActionType { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
            
        // Navigation property
        public Card Card { get; set; }
        public BoardList BoardList { get; set; }
    }
}
