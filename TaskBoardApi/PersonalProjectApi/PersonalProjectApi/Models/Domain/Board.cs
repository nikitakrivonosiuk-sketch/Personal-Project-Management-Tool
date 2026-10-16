namespace PersonalProjectApi.Models.Domain
{
    public class Board : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public ICollection<BoardList> BoardLists { get; set; } = new List<BoardList>();
    }
}
