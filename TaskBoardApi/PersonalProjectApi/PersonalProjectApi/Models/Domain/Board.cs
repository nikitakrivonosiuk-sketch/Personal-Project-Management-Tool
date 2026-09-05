namespace PersonalProjectApi.Models.Domain
{
    public class Board
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public ICollection<BoardList>? BoardLists { get; set; } = new List<BoardList>();
    }
}
