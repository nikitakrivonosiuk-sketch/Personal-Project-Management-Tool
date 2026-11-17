namespace PersonalProjectApi.Models.Domain
{
    public class BoardList : BaseEntity
    {
        public Guid BoardId { get; set; }
        public string Title { get; set; } = string.Empty;
        public int Position { get; set; }
        public Board Board { get; set; }
        public ICollection<Card> Cards { get; set; } = new List<Card>();
    }
}
