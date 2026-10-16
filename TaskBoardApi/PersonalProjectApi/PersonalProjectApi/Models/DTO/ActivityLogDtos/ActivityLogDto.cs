using PersonalProjectApi.Models.Domain;

namespace PersonalProjectApi.Models.DTO.ActivityLogDtos
{
    public class ActivityLogDto
    {
        public Guid Id { get; set; }
        public Guid? CardId { get; set; }
        public Guid? BoardListId { get; set; }
        public Guid? BoardId { get; set; }
        public string Description { get; set; }
        public ActionType ActionType { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
