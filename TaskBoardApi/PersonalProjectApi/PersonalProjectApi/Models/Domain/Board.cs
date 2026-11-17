namespace PersonalProjectApi.Models.Domain
{
    public class Board : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public Guid OwnerId { get; set; }
        public AppUser Owner { get; set; }
        public ICollection<BoardMember> BoardMembers { get; set; } = new List<BoardMember>();
        public ICollection<BoardList> BoardLists { get; set; } = new List<BoardList>();
    }
}
