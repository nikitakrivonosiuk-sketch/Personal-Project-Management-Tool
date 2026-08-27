namespace PersonalProjectApi.Models.DTO.ActivityLogDtos
{
    public class ActivityLogDto
    {
        public Guid Id { get; set; }
        public Guid? CardId { get; set; }
        public Guid? BoardListId { get; set; }
        public string Description { get; set; }
        public string ActionType { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
