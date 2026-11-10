using PersonalProjectApi.Models.Enums;

namespace PersonalProjectApi.Models.Domain
{
    public class BoardMember
    {
        public Guid UserId { get; set; }
        public Guid BoardId { get; set; }
        public UserLevelAccess UserLevel { get; set; }

        public Board Board { get; set; }
        public AppUser User { get; set; }
    }
}
