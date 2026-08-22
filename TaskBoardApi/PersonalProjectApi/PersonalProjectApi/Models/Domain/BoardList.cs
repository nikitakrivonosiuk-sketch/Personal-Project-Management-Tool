namespace PersonalProjectApi.Models.Domain
{
    public class BoardList
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public ICollection<Card>? Cards { get; set; } = new List<Card>();
    }
}
