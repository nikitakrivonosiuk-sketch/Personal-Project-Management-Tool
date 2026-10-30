namespace PersonalProjectApi.Models.Domain
{
    public class User : BaseEntity
    {
        public string? Email { get; set; }
        public int Password { get; set; }
    }
}
